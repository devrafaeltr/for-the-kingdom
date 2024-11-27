using UnityEngine;

namespace FTKingdom
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D projectileRigidbody = null;
        [SerializeField] private SpriteRenderer projectileRenderer = null;

        protected ProjectileSO projectileData;
        private Transform target;

        public void Setup(ProjectileSO projectile, Transform newTarget)
        {
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Checa a tag para verificar se atingiu um inimigo
            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent(out CharacterBattle enemy))
                {
                    enemy.DoDamage(projectileData.Damage); // Aplica o dano ao inimigo
                }

                // TODO: Release to pool
                Destroy(gameObject); // Destroi o projétil após a colisão
            }
        }
    }
}