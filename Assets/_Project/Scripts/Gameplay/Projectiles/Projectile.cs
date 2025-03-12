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
        protected Transform target;
        protected int damage;

        public void Setup(int damage, CharacterType type, ProjectileSO data, Transform target)
        {
            this.damage = damage;
            userType = type;
            this.target = target;

            projectileData = data;

            projectileRenderer.sprite = projectileData.Graphic;
            animator.runtimeAnimatorController = projectileData.AnimatorOverrider;

            StartAnimation();
            OnSetup();
        }

        private void Update()
        {
            if (target == null)
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
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            projectileRigidbody.linearVelocity = direction.normalized * projectileData.Speed;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform == target)
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
            projectileTarget.DoDamage(damage);
        }
    }
}