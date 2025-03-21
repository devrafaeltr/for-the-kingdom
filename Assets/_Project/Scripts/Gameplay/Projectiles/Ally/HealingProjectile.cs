using System.Linq;

namespace FTKingdom
{
    public class HealingProjectile : AllyProjectile
    {
        protected override void OnSetup()
        {
            base.OnSetup();
            var target = possibleTargets.OrderByDescending(t => t.MissingHealthPercent).ToList()[0].Transform;
            hpModifierData.SetTarget(target);
        }

        protected override void OnFindTarget(CharacterBattle projectileTarget)
        {
            projectileTarget.ApplyHealthPointsModifier(hpModifierData);
        }
    }
}
