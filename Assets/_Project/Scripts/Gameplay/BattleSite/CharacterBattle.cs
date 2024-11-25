using FTKingdom.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace FTKingdom
{
    public class CharacterBattle : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] protected SpriteRenderer spriteRenderer;

        public Transform auxTarget;

        private readonly int maxMana = 0;
        private readonly int maxHp = 0;
        private int currentMana = 0;
        private int currentHp = 0;

        #region Movement variables
        private const float StuckThreshold = 0.5f;
        private const float StuckCheckInterval = 1f;
        private float stuckTimer;
        private Vector3 lastPosition;
        #endregion Movement variables

        private void Awake()
        {
            SetupNavmesh();
        }

        private void Update()
        {
            if (navMeshAgent.isStopped)
            {
                return;
            }

            CheckStuck();
        }

        private void OnEnable()
        {
            EventsManager.AddListener(EventsManager.OnBattleStart, StartBattle);
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnBattleStart, StartBattle);
        }

        protected virtual void OnSetup()
        { }

        protected virtual void OnStartBattle()
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

        protected void FindEnemy()
        {
            auxTarget = BattleSiteManager.Instance.GetClosestFromType(transform.position, CharacterType.Enemy).transform;
        }

        private void ConsumeMana(int quantity)
        {
            UpdatePointsBar(maxMana, currentMana - quantity);
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
            float percent = (float)max / current;
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

        private void StartBattle(IGameEvent gameEvent)
        {
            OnStartBattle();
        }
    }
}