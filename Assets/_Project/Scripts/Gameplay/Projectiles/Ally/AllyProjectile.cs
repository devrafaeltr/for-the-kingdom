using System.Collections.Generic;

namespace FTKingdom
{
    public class AllyProjectile : Projectile
    {
        protected List<CharacterBattle> possibleTargets = null;

        protected override void OnSetup()
        {
            if (hpModifierData.AttackerType == CharacterType.Enemy)
            {
                possibleTargets = BattleSiteManager.Instance.EnemiesInBattle;
            }
            else
            {
                possibleTargets = BattleSiteManager.Instance.HeroesInBattle;
            }
        }
    }
}
