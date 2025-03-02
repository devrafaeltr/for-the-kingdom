using DG.Tweening;
using UnityEngine;

namespace FTKingdom
{
    public class CharacterStateAttacking : IState
    {
        private float attackTimer = 0f;

        internal override CharacterState StateType => CharacterState.Attacking;

        internal override void End(CharacterBattle entity)
        { }

        internal override void Start(CharacterBattle entity)
        {
            attackTimer = entity.CharacterData.BaseAttackInterval;
        }

        internal override void Update(CharacterBattle entity)
        {
            HandleAttack(entity);
        }

        private void HandleAttack(CharacterBattle entity)
        {
            if (!entity.IsCloseToTarget())
            {
                entity.MoveTowardsTarget();
                return;
            }

            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                PerformAttack(entity);
                attackTimer = entity.CharacterData.BaseAttackInterval;
            }
        }

        private void PerformAttack(CharacterBattle entity)
        {
            // Debug.Log($"{gameObject.name} attacks {auxTarget.name} for {characterData.BaseDamage} damage!");
            // Debug.Log($"{characterData.Name} attacks {auxTarget.name}! {characterData.BaseAttackDistance} | {characterData.AttackType}");

            if (entity.CharacterData.AttackType == CharacterAttackType.Melee)
            {
                DoMeleeAttack(entity);
            }
            else
            {
                entity.SetAnimationTrigger("Attack");
                entity.SpawnProjectile();
            }
        }

        private void DoMeleeAttack(CharacterBattle entity)
        {
            Vector3 originalPosition = entity.Transform.position;
            Vector3 attackPosition = entity.Target.position;
            Vector3 direction = (attackPosition - originalPosition).normalized;
            Vector3 moveBackPosition = originalPosition - direction * 0.5f;

            Sequence attackSequence = DOTween.Sequence();
            attackSequence.Append(entity.Transform.DOMove(moveBackPosition, 0.1f))
                          .Append(entity.Transform.DOMove(attackPosition, 0.2f))
                          .OnComplete(() =>
                          {
                              entity.SpawnProjectile();
                          })
                          .Append(entity.Transform.DOMove(originalPosition, 0.1f));
        }
    }
}