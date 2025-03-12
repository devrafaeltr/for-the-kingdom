using System.Collections.Generic;
using System.Linq;

namespace FTKingdom
{
    public class AllyProjectile : Projectile
    {
        protected override void OnSetup()
        {
            List<CharacterBattle> targets = null;
            if (userType == CharacterType.Enemy)
            {
                targets = BattleSiteManager.Instance.EnemiesInBattle;
            }
            else
            {
                targets = BattleSiteManager.Instance.HeroesInBattle;
            }

            // TODO: Fix for another-kind skills. 
            // This is only for healing
            targets.OrderBy(t => t.GetHealth());
            target = targets[0].Transform;
            UnityEngine.Debug.Log(string.Join(", ", targets.Select(t => $"({t.GetHealth()} | {t.CharacterData.Name})")));
        }

        protected override void OnFindTarget(CharacterBattle projectileTarget)
        {
            // TODO: Fix for another-kind skills. 
            // This is only for healing
            projectileTarget.DoDamage(-damage);
        }
    }
}
