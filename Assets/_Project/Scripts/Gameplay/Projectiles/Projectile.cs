using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private Rigidbody2D projectileRigidbody = null;
        [SerializeField] private SpriteRenderer projectileRenderer = null;
        [SerializeField] private Collider2D projectileCollider = null;

        protected ProjectileSO projectileData;
        protected HPModifierData hpModifierData;

        private void OnEnable()
        {
            EventsManager.AddListener(EventsManager.OnCharacterDie, OnCharacterDie);
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnCharacterDie, OnCharacterDie);
        }

        public void Setup(HPModifierData modifierData, ProjectileSO porjectileData, SpellBehaviorType behaviorType)
        {
            hpModifierData = modifierData;
            projectileData = porjectileData;

            projectileRenderer.sprite = projectileData.Graphic;
            animator.runtimeAnimatorController = projectileData.AnimatorOverrider;

            StartAnimation();
            OnSetup();

            if (behaviorType == SpellBehaviorType.Instant)
            {
                transform.position = hpModifierData.TargetTransform.position;
            }

            projectileCollider.enabled = true;
        }

        private void Update()
        {
            if (hpModifierData.TargetTransform == null)
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

        // Used by animation event
        private void DoEndAnimaion()
        {
            if (projectileData.AnimatorOverrider != null)
            {
                animator.SetBool("End", true);
            }
        }

        private void MoveTowardsTarget()
        {
            Vector3 direction = hpModifierData.TargetTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            projectileRigidbody.linearVelocity = direction.normalized * projectileData.Speed;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform == hpModifierData.TargetTransform)
            {
                if (collider.TryGetComponent(out CharacterBattle projectileTarget))
                {
                    OnFindTarget(projectileTarget);
                }

                // TODO: Release to pool
                Destroy(gameObject); // Destroi o projétil após a colisão
                projectileCollider.enabled = false;
            }
        }

        protected virtual void OnSetup()
        { }

        protected virtual void OnFindTarget(CharacterBattle projectileTarget)
        {
            projectileTarget.ApplyHealthPointsModifier(hpModifierData);
        }

        private void OnCharacterDie(IGameEvent gameEvent)
        {
            OnCharacterDieEvent onCharacterDieEvent = (OnCharacterDieEvent)gameEvent;

            if (onCharacterDieEvent.Character == hpModifierData.TargetTransform)
            {
                hpModifierData.SetTarget(null);
            }
        }
    }
}