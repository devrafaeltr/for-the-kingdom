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
                    FindTarget();
                }

                return target;
            }
            private set => target = value;
        }

        [HideInInspector] public Transform Transform { get; private set; }

        private readonly int maxMana = 0;
        private readonly int maxHp = 0;
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
            navMeshAgent.SetDestination(Target.position);
        }

        public void AttackTarget()
        {
            ChangeState(CharacterState.Attacking);
        }

        public bool IsCloseToTarget()
        {
            return Vector3.Distance(Transform.position, Target.position) <= CharacterData.BaseAttackDistance;
        }

        protected virtual void OnSetup()
        {
            currentHp = CharacterData.BaseHp;
            currentMana = CharacterData.BaseMp;

            navMeshAgent.stoppingDistance = CharacterData.BaseAttackDistance;
        }

        private void Damage(int damage)
        {
            currentHp -= damage;

            if (currentHp > 0)
            {
                UpdatePointsBar(maxHp, currentHp);
                return;
            }

            Die();
        }

        private void FindTarget()
        {
            CharacterType targetype = CharacterData.Type == CharacterType.Hero ? CharacterType.Enemy : CharacterType.Hero;
            Target = BattleSiteManager.Instance.GetClosestFromType(transform.position, targetype).transform;
        }

        private void ConsumeMana(int quantity)
        {
            currentMana -= quantity;
            UpdatePointsBar(maxMana, currentMana);
        }

        private void SetupNavmesh()
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void UpdatePointsBar(int max, int current)
        {
            float percent = (float)max / current;
        }

        private void Die()
        {
            currentHp = 0;
            // Debug.Log($"{gameObject.name} has died!");
        }
    }
}