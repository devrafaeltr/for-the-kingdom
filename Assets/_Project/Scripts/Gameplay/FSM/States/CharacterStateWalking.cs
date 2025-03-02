using UnityEngine;

namespace FTKingdom
{
    public class CharacterStateWalking : IState
    {
        internal override CharacterState StateType => CharacterState.Walking;


        #region Movement variables
        private const float StuckThreshold = 0.5f;
        private const float StuckCheckInterval = 1f;
        private float stuckTimer;
        private Vector3 lastPosition;
        private float distanceToTarget = 0;
        #endregion Movement variables

        internal override void End(CharacterBattle entity)
        {
            entity.SetAnimationBool("Walk", false);
        }

        internal override void Start(CharacterBattle entity)
        {
            entity.SetAnimationBool("Walk", true);
        }

        internal override void Update(CharacterBattle entity)
        {
            if (entity.IsCloseToTarget())
            {
                entity.AttackTarget();
            }
            else if (entity.IsPathBlocked())
            {
                entity.WalkToTarget();
                CheckStuck(entity);
            }
        }

        private void CheckStuck(CharacterBattle entity)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= StuckCheckInterval)
            {
                stuckTimer = 0;
                float distanceMoved = Vector3.Distance(entity.Transform.position, lastPosition);

                if (distanceMoved < StuckThreshold)
                {
                    entity.StopAgent();
                }

                lastPosition = entity.Transform.position;
            }
        }
    }
}