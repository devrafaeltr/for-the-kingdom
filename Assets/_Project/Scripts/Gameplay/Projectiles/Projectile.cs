using UnityEngine;

namespace FTKingdom
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private Rigidbody2D projectileRigidbody = null;
        [SerializeField] private SpriteRenderer projectileRenderer = null;

        protected ProjectileSO projectileData;
        private Transform target;
        private int damage;

        public void Setup(int projectileDamage, ProjectileSO projectile, Transform newTarget)
        {
            damage = projectileDamage;
            projectileData = projectile;

            projectileRenderer.sprite = projectile.Graphic;
            target = newTarget;

            animator.runtimeAnimatorController = projectileData.AnimatorOverrider;
            StartAnimation();
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
                if (collider.TryGetComponent(out CharacterBattle enemy))
                {
                    enemy.DoDamage(damage);
                }

                // TODO: Release to pool
                Destroy(gameObject); // Destroi o projétil após a colisão
            }
        }
    }
}