using UnityEngine;

namespace FTKingdom
{
    public class AnimationEventHandler : MonoBehaviour
    {
        [SerializeField] private CharacterBattle characterBattle;

        // Used by animator.
        private void OnAfterAttack()
        {
            characterBattle.SpawnRangedProjectile();
        }
    }
}