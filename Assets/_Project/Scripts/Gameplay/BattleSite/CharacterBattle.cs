using FTKingdom.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace FTKingdom
{
    public class CharacterBattle : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected CharacterSO characterData = null;

        [Header("Attack info")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private ProjectileSO projectileData;

        public Transform auxTarget;
        private float attackTimer;

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

        private bool auxCanFight = false;

        private void Awake()
        {
            SetupNavmesh();
        }

        private void Update()
        {
            if (!auxCanFight)
            {
                return;
            }

            if (auxTarget == null)
            {
                FindEnemy();
                return;
            }

            if (!navMeshAgent.hasPath && !navMeshAgent.pathPending)
            {
                HandleAttack();
            }
            // if (navMeshAgent.isStopped)
            // {
            //     HandleAttack();
            // }
            else
            {
                CheckStuck();
            }
        }

        private void OnEnable()
        {
            EventsManager.AddListener(EventsManager.OnBattleStart, StartBattle);
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnBattleStart, StartBattle);
        }

        public void DoDamage(int damage)
        {
            Damage(damage);
        }

        protected virtual void OnSetup()
        { }

        protected virtual void OnStartBattle()
        {
            navMeshAgent.SetDestination(auxTarget.position);
            attackTimer = characterData.BaseAttackInterval;
            auxCanFight = true;
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
            currentMana -= quantity;
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
            navMeshAgent.ResetPath();
        }

        private void UpdatePointsBar(int max, int current)
        {
            float percent = (float)max / current;
        }

        private void HandleAttack()
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= characterData.BaseAttackInterval)
            {
                PerformAttack();
                attackTimer = 0f;
            }
        }

        private void PerformAttack()
        {
            if (auxTarget != null)
            {
                Debug.Log($"{gameObject.name} attacks {auxTarget.name} for {characterData.BaseDamage} damage!");
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<Projectile>().Setup(projectileData, auxTarget);
            }
        }

        private void Die()
        {
            currentHp = 0;
            Debug.Log($"{gameObject.name} has died!");
        }

        private void StartBattle(IGameEvent gameEvent)
        {
            OnStartBattle();
        }
    }
}