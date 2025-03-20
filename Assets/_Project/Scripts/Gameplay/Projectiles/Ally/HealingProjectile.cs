using System.Linq;

namespace FTKingdom
{
    public class HealingProjectile : AllyProjectile
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            currentTarget = possibleTargets.OrderByDescending(t => t.MissingHealthPercent).ToList()[0].Transform;
        }

        protected override void OnFindTarget(CharacterBattle projectileTarget)
        {
            projectileTarget.ApplyHelathPointsModifier(-damage);
        }
    }
}
