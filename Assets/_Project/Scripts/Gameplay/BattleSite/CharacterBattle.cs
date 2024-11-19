using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace FTKingdom
{
    public class CharacterBattle : MonoBehaviour
    {
        public CharacterSO characerData;
        [SerializeField] protected NavMeshAgent navMeshAgent;
        public Transform auxTarget;

        private int maxMana = 0;
        private int maxHp = 0;
        private int currentMana = 0;
        private int currentHp = 0;

        private CharacterType type;

        #region Movement variables
        private const float StuckThreshold = 0.7f;
        private const float StuckCheckInterval = 0.5f;
        private float stuckTimer;
        private Vector3 lastPosition;
        #endregion Movement variables

        private void Start()
        {
            SetupNavmesh();
            Setup();
        }

        private void Update()
        {
            if (navMeshAgent.isStopped)
            {
                return;
            }

            CheckStuck();
        }

        public void Setup()
        {
            FindEnemy();
            OnSetup();
        }

        protected virtual void OnSetup()
        {
            navMeshAgent.SetDestination(auxTarget.position);
        }

        protected virtual void Damage(int damage)
        {
            currentHp -= damage;

            if (currentHp > 0)
            {
                UpdatePointsBar(maxHp, currentHp);
                return;
            }

            Die();
        }

        private void FindEnemy()
        {
            auxTarget = GameplayManager.Instance.GetClosestFromType(transform.position, CharacterType.Enemy).transform;
        }

        private void ConsumeMana(int quantity)
        {
            UpdatePointsBar(maxMana, currentMana);
        }

        private void SetupNavmesh()
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void CheckStuck()
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= StuckCheckInterval)
            {
                stuckTimer = 0;
                float distanceMoved = Vector3.Distance(transform.position, lastPosition);

                if (distanceMoved < StuckThreshold)
                {
                    StopAgent();
                }

                lastPosition = transform.position;
            }
        }

        private void StopAgent()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath(); // Clear the target
        }

        private void UpdatePointsBar(int max, int current)
        {
            float percent = max / current;
        }

        private void PerformAttack()
        {
            // Implement your attack logic here, e.g., deal damage to the target
            Debug.Log($"{gameObject.name} attacks {auxTarget.name}!");
        }

        private void Die()
        {
            currentHp = 0;
        }
    }
}