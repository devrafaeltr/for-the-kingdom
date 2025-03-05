using UnityEngine;

namespace FTKingdom
{
    public class Projectile : MonoBehaviour
    {
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

        private void MoveTowardsTarget()
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            projectileRigidbody.linearVelocity = direction.normalized * projectileData.Speed;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log($"{transform.name} collided with {collider.transform.name} | {target.name}");
            // if (collision.CompareTag("Enemy"))
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