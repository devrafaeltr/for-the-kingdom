using DG.Tweening;
using FTKingdom.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace FTKingdom
{
    public class CharacterBattle : MonoBehaviour
    {
        // TODO: Improve how walk animation are being set.
        [SerializeField] private Animator characterAnimator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected CharacterSO characterData = null;

        [Header("Attack info")]
        [SerializeField] private Transform projectileSpawnPosition;
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

        private bool hasBattleStarted = false;

        private void Awake()
        {
            SetupNavmesh();

            if (characterData.Type == CharacterType.Enemy)
            {
                OnSetup();
            }
        }

        private void Update()
        {
            if (!hasBattleStarted)
            {
                return;
            }

            if (auxTarget == null)
            {
                FindTarget();
                return;
            }

            float distanceToTarget = Vector3.Distance(transform.position, auxTarget.position);
            if (distanceToTarget > characterData.BaseAttackDistance)
            {
                MoveTowardsTarget();
            }
            else
            {
                if (!navMeshAgent.hasPath && !navMeshAgent.pathPending)
                {
                    characterAnimator.SetBool("Walk", false);
                    HandleAttack();
                }
                else
                {
                    CheckStuck();
                }
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
        {
            currentHp = characterData.BaseHp;
            currentMana = characterData.BaseMp;

            navMeshAgent.stoppingDistance = characterData.BaseAttackDistance;
        }

        private void OnStartBattle()
        {
            attackTimer = characterData.BaseAttackInterval;
            hasBattleStarted = true;
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
            CharacterType targetype = characterData.Type == CharacterType.Hero ? CharacterType.Enemy : CharacterType.Hero;
            auxTarget = BattleSiteManager.Instance.GetClosestFromType(transform.position, targetype).transform;
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
                // Debug.Log($"{gameObject.name} attacks {auxTarget.name} for {characterData.BaseDamage} damage!");
                // Debug.Log($"{characterData.Name} attacks {auxTarget.name}! {characterData.BaseAttackDistance} | {characterData.AttackType}");

                if (characterData.AttackType == CharacterAttackType.Melee)
                {
                    DoMeleeAttack();
                }
                else
                {
                    characterAnimator.SetTrigger("Attack");
                    SpawnProjectile();
                }
            }
            else
            {
                FindTarget();
            }
        }

        private void DoMeleeAttack()
        {
            Vector3 originalPosition = transform.position;
            Vector3 attackPosition = auxTarget.position;
            Vector3 direction = (attackPosition - originalPosition).normalized;
            Vector3 moveBackPosition = originalPosition - direction * 0.5f;

            Sequence attackSequence = DOTween.Sequence();
            attackSequence.Append(transform.DOMove(moveBackPosition, 0.1f))
                          .Append(transform.DOMove(attackPosition, 0.2f))
                          .OnComplete(() =>
                          {
                              SpawnProjectile();
                          })
                          .Append(transform.DOMove(originalPosition, 0.1f));
        }

        private void SpawnProjectile()
        {
            GameObject projectile = Instantiate(projectileData.ProjectilePrefab, projectileSpawnPosition.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Setup(characterData.BaseDamage, projectileData, auxTarget);
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

        private void MoveTowardsTarget()
        {
            characterAnimator.SetBool("Walk", true);
            navMeshAgent.SetDestination(auxTarget.position);
        }
    }
}