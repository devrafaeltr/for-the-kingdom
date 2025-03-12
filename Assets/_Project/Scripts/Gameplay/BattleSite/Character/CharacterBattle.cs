using System;
using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace FTKingdom
{
    public class CharacterBattle : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private SpriteRenderer spriteRenderer;
        internal CharacterSO CharacterData = null;

        [Header("Attack info")]
        [SerializeField] private CharacterBarPointController characterBarPointController;
        [SerializeField] private Transform projectileSpawnPositionParent;
        [SerializeField] private Transform projectileSpawnPosition;

        private List<CharacterSpell> characterSpells = new();

        public Transform SpriteTransform => spriteRenderer.transform;
        private Transform target;
        public Transform Target
        {
            get
            {
                if (target == null)
                {
                    target = FindTarget();

                    if (target == null)
                    {
                        ChangeState(CharacterState.Waiting);
                    }
                }

                return target;
            }
            private set => target = value;
        }

        internal Transform Transform { get; private set; }

        private int maxMana = 0;
        private int maxHp = 0;
        private int currentMana = 0;
        private int currentHp = 0;

        // TODO: Change to BaseeFSMController<T>
        private readonly BaseFSMController characterFSM = new();
        private CharacterState currenState = CharacterState.Waiting;

        // private readonly Dictionary<SpellTrigger, CharacterSpell> spellByTriggers = new();

        private void Start()
        {
            Transform = transform;
            characterFSM.InitializeStates(this, CharacterState.Waiting);

            SetupNavmesh();
        }

        private void OnEnable()
        {
            EventsManager.AddListener(EventsManager.OnCharacterDie, OnCharacterDie);
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnCharacterDie, OnCharacterDie);
            characterFSM.ForceEndState(this);
        }

        private void Update()
        {
            characterFSM.UpdateCurrentState();
        }

        internal void SpawnRangedProjectile()
        {
            SpawnProjectile();
        }

        public int GetHealth()
        {
            return currentHp;
        }

        public void Setup(CharacterSO character)
        {
            OnSetup(character);
        }

        public void SetAnimationBool(string animation, bool value)
        {
            characterAnimator.SetBool(animation, value);
        }

        public void SetAnimationTrigger(string animation)
        {
            characterAnimator.SetTrigger(animation);
        }

        public void DoDamage(int damage)
        {
            Damage(damage);
        }

        public void StopAgent()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }

        public bool IsPathBlocked()
        {
            return navMeshAgent.hasPath || navMeshAgent.pathPending;
        }

        public void SpawnProjectile()
        {
            if (target == null || projectileSpawnPosition == null)
            {
                return;
            }

            if (CharacterData.AttackType == CharacterAttackType.Melee)
            {
                projectileSpawnPosition.position = Target.position;
            }

            RotateSpawnPosition();
            GameObject projectile = Instantiate(CharacterData.ProjectileData.ProjectilePrefab, projectileSpawnPosition.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Setup(CharacterData.BaseDamage, CharacterData.Type, CharacterData.ProjectileData, Target);
        }

        public void MoveTowardsTarget()
        {
            ChangeState(CharacterState.Walking);
            WalkToTarget();
        }

        public void WalkToTarget()
        {
            if (Target == null)
            {
                return;
            }

            navMeshAgent.SetDestination(Target.position);
        }

        public void AttackTarget()
        {
            ChangeState(CharacterState.Attacking);
        }

        public bool IsCloseToTarget()
        {
            if (Target == null)
            {
                return false;
            }

            return Vector3.Distance(Transform.position, Target.position) <= CharacterData.BaseAttackDistance;
        }

        public void DoDeathFlow()
        {
            StopAgent();
            characterBarPointController.Disable();
            navMeshAgent.enabled = false;
        }

        protected virtual void OnSetup(CharacterSO characterSO)
        {
            CharacterData = characterSO;

            spriteRenderer.sprite = CharacterData.Graphic;
            currentHp = maxHp = CharacterData.BaseHp;
            currentMana = maxMana = CharacterData.BaseMp;
            navMeshAgent.stoppingDistance = CharacterData.BaseAttackDistance;

            SetupSpells();
        }

        protected virtual void OnDie()
        {
            BattleSiteManager.Instance.RemoveEnemy(this);
        }

        #region Spells
        private void SetupSpells()
        {
            foreach (var spellData in CharacterData.PossibleSpells)
            {
                characterSpells.Add(new CharacterSpell(spellData, CharacterData.Type));
            }
        }

        internal void SetupSpellsCooldown()
        {
            foreach (var spell in characterSpells)
            {
                spell.ResetCooldown();
            }
        }

        internal void UpdateSpellCooldown()
        {
            foreach (var spell in characterSpells)
            {
                spell.DoCooldownProgress();

                if (spell.CanUse)
                {
                    spell.Use(transform.position, Target);
                }
            }
        }
        #endregion

        private void RotateSpawnPosition()
        {
            Vector3 direction = target.position - projectileSpawnPosition.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectileSpawnPositionParent.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void Damage(int damage)
        {
            currentHp = Mathf.Clamp(currentHp - damage, 0, maxHp);
            characterBarPointController.UpdateHealthPoints(currentHp, maxHp);

            if (currentHp <= 0)
            {
                Die();
            }
        }

        private Transform FindTarget()
        {
            CharacterType targetype = CharacterData.Type == CharacterType.Hero ? CharacterType.Enemy : CharacterType.Hero;
            var target = BattleSiteManager.Instance.GetClosestFromType(transform.position, targetype);
            return target != null ? target : null;
        }

        private void ConsumeMana(int quantity)
        {
            currentMana -= quantity;
            characterBarPointController.UpdateManaPoints(currentMana, maxMana);
        }

        private void SetupNavmesh()
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void Die()
        {
            currentHp = 0;
            ChangeState(CharacterState.Dead);

            OnDie();

            EventsManager.Publish(EventsManager.OnCharacterDie, new OnCharacterDieEvent(Transform));
        }

        private void OnCharacterDie(IGameEvent gameEvent)
        {
            OnCharacterDieEvent characterDieEvent = (OnCharacterDieEvent)gameEvent;

            if (characterDieEvent.Character != Target)
            {
                return;
            }

            Target = FindTarget();
        }

        private void ChangeState(CharacterState state)
        {
            currenState = state;
            characterFSM.ChangeState(state);
        }
    }
}