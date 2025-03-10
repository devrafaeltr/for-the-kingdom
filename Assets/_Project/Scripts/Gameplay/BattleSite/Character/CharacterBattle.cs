using FTKingdom.Utils;
using JetBrains.Annotations;
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
        [SerializeField] private ProjectileSO projectileData;

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

        internal void SpawnRangedProjectile()
        {
            SpawnProjectile();
        }

        private void Update()
        {
            characterFSM.UpdateCurrentState();
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

            RotateSpawnPosition();
            GameObject projectile = Instantiate(projectileData.ProjectilePrefab, projectileSpawnPosition.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Setup(CharacterData.BaseDamage, projectileData, Target);
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
        }

        protected virtual void OnDie()
        {
            BattleSiteManager.Instance.RemoveEnemy(transform);
        }

        private void RotateSpawnPosition()
        {
            Vector3 direction = target.position - projectileSpawnPosition.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectileSpawnPositionParent.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void Damage(int damage)
        {
            currentHp -= damage;

            if (currentHp > 0)
            {
                characterBarPointController.UpdateHealthPoints(currentHp, maxHp);
                return;
            }

            characterBarPointController.UpdateHealthPoints(Mathf.Max(currentHp, 0), maxHp);
            Die();
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