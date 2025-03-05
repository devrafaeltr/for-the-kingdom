using UnityEngine;

namespace FTKingdom
{
    public class CharacterStateDead : IState
    {
        internal override CharacterState StateType => CharacterState.Attacking;

        internal override void End(CharacterBattle entity)
        { }

        internal override void Start(CharacterBattle entity)
        {
            entity.DisablePointBars();
            entity.SetAnimationTrigger("Die");
        }

        internal override void Update(CharacterBattle entity)
        { }
    }
}