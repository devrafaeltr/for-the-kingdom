using UnityEngine;

namespace FTKingdom
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private Rigidbody2D projectileRigidbody = null;
        [SerializeField] private SpriteRenderer projectileRenderer = null;

        protected CharacterType userType = CharacterType.Enemy;
        protected ProjectileSO projectileData;
        protected Transform currentTarget;
        protected int damage;

        public void Setup(int hpDiff, CharacterType type, ProjectileSO data, Transform target, SpellBehaviorType behaviorType = SpellBehaviorType.Default)
        {
            damage = hpDiff;
            userType = type;
            currentTarget = target;

            projectileData = data;

            projectileRenderer.sprite = projectileData.Graphic;
            animator.runtimeAnimatorController = projectileData.AnimatorOverrider;

            StartAnimation();
            OnSetup();

            if (behaviorType == SpellBehaviorType.Instant)
            {
                transform.position = currentTarget.position;
            }
        }

        private void Update()
        {
            if (currentTarget == null)
            {
                // TODO: Release to pool
                Destroy(gameObject);
                return;
            }

            MoveTowardsTarget();
        }

        private void StartAnimation()
        {
            if (projectileData.AnimatorOverrider != null)
            {
                animator.SetBool("Start", true);
            }
        }

        // private void DoSpawnAnimation()
        // {
        //     // Possibly its not needed.
        // }

        private void DoEndAnimaion()
        {
            if (projectileData.AnimatorOverrider != null)
            {
                animator.SetBool("End", true);
            }
        }

        private void MoveTowardsTarget()
        {
            Vector3 direction = currentTarget.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            projectileRigidbody.linearVelocity = direction.normalized * projectileData.Speed;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform == currentTarget)
            {
                if (collider.TryGetComponent(out CharacterBattle projectileTarget))
                {
                    OnFindTarget(projectileTarget);
                }

                // TODO: Release to pool
                Destroy(gameObject); // Destroi o projétil após a colisão
            }
        }

        protected virtual void OnSetup()
        { }

        protected virtual void OnFindTarget(CharacterBattle projectileTarget)
        {
            projectileTarget.ApplyHelathPointsModifier(damage);
        }
    }
}