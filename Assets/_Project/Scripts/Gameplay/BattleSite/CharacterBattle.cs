using FTKingdom.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace FTKingdom
{
    public class CharacterBattle : MonoBehaviour
    {
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] public CharacterSO CharacterData = null;

        [Header("Attack info")]
        [SerializeField] private CharacterBarPointController characterBarPointController;
        [SerializeField] private Transform projectileSpawnPosition;
        [SerializeField] private ProjectileSO projectileData;

        private Transform target;

        [HideInInspector]
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

        [HideInInspector] public Transform Transform { get; private set; }

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

            // TODO: Remove when implementing enemy instantiation
            if (CharacterData != null)
            {
                OnSetup();
            }
        }

        private void OnEnable()
        {
            EventsManager.AddListener(EventsManager.OnCharacterDie, OnCharacterDie);
        }

        private void OnDisable()
        {
            EventsManager.AddListener(EventsManager.OnCharacterDie, OnCharacterDie);
        }

        private void Update()
        {
            characterFSM.UpdateCurrentState();
        }

        private void ChangeState(CharacterState state)
        {
            currenState = state;
            characterFSM.ChangeState(state);
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

        public void DisablePointBars()
        {
            characterBarPointController.Disable();
        }

        protected virtual void OnSetup()
        {
            currentHp = maxHp = CharacterData.BaseHp;
            currentMana = maxMana = CharacterData.BaseMp;

            navMeshAgent.stoppingDistance = CharacterData.BaseAttackDistance;
        }

        protected virtual void OnDie()
        {
            BattleSiteManager.Instance.UpdateEnemies(transform);
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
    }
}