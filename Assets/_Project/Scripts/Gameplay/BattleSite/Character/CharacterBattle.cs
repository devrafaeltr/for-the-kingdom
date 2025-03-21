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

        private readonly List<CharacterSpell> characterSpells = new();
        public List<CharacterSpell> CharacterSpells { get => characterSpells; }

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

        internal int CharacterPosition { get; set; }
        internal Transform Transform { get; private set; }
        internal float MissingHealthPercent => 1 - (currentHp / (float)maxHp);

        private int maxHp = 0;
        private int currentHp = 0;

        // TODO: Change to BaseeFSMController<T>
        private readonly BaseFSMController characterFSM = new();
        protected CharacterState currenState = CharacterState.Waiting;

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

        public void ApplyHealthPointsModifier(HPModifierData hpModifierData)
        {
            ApplyModifierInternal(hpModifierData);
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

            // TODO: Change base attack to magical if needed for some classes (mage, etc)

            int dmg = GetDamage();
            DamageModifier damageModifier = dmg > CharacterData.BaseDamage ? DamageModifier.Critical : DamageModifier.Normal;
            HPModifierData hpModifierData = new(dmg, DamageType.Physical, damageModifier,
            CharacterData.Type, Transform, Target);

            projectile.GetComponent<Projectile>().Setup(hpModifierData, CharacterData.ProjectileData, SpellBehaviorType.Default);
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
            // TODO: Not here, but get random spells from PossibleSpells, not every spell
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
                    // TODO: Calculate spell critial 
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

        private void ApplyModifierInternal(HPModifierData modifier)
        {
            if (currentHp <= 0)
            {
                return;
            }

            if (modifier.Value > 0)
            {
                DoDamage(modifier);
            }
            else
            {
                DoHeal(-modifier.Value);
            }
        }

        private void DoDamage(HPModifierData damageModifier)
        {
            int finalDamage = damageModifier.Value;

            if (damageModifier.Type == DamageType.Physical)
            {
                if (DodgedAttack())
                {
                    ShowText($"MISS", damageModifier.AttackerTransform.position, TextType.Evasion);
                    return;
                }

                finalDamage -= CharacterData.BaseDefense;
            }

            if (damageModifier.Modifier == DamageModifier.Critical)
            {
                UpdateHealthPoints(finalDamage, TextType.Critical);
            }
            else
            {
                UpdateHealthPoints(finalDamage, TextType.Damage);
            }

            if (currentHp <= 0)
            {
                Die();
            }
        }

        private void DoHeal(int heal)
        {
            UpdateHealthPoints(heal, TextType.Heal);
        }

        private bool DodgedAttack()
        {
            return Random.Range(0, 100) < CharacterData.BaseEvasionRate;
        }

        private void UpdateHealthPoints(int modifier, TextType textType)
        {
            switch (textType)
            {
                case TextType.Damage:
                    ShowText($"-{modifier}", Transform.position, textType);
                    break;
                case TextType.Heal:
                    ShowText($"+{Mathf.Abs(modifier)}", Transform.position, textType);
                    break;
            }

            FloatingTextManager.Instance.Show($"{modifier}", Transform.position, textType);
            currentHp = Mathf.Clamp(currentHp - modifier, 0, maxHp);
            characterBarPointController.UpdateHealthPoints(currentHp, maxHp);
            BattleSiteCanvas.Instance.UpdateHealth(this);
        }

        private void ShowText(string text, Vector3 spawnPo, TextType textType)
        {
            FloatingTextManager.Instance.Show(text, spawnPo, textType);
        }

        private int GetDamage()
        {
            int criticalChance = Random.Range(0, 100);
            if (criticalChance < CharacterData.BaseCriticalRate)
            {
                return CharacterData.BaseDamage * 2;
            }

            return CharacterData.BaseDamage;
        }

        private Transform FindTarget()
        {
            CharacterType targetype = CharacterData.Type == CharacterType.Hero ? CharacterType.Enemy : CharacterType.Hero;
            var target = BattleSiteManager.Instance.GetClosestFromType(transform.position, targetype);
            return target != null ? target : null;
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