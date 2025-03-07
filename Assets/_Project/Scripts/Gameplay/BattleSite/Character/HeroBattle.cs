using FTKingdom.Utils;

namespace FTKingdom
{
    public class HeroBattle : CharacterBattle
    {
        protected HeroData characterInfos = null;

        public void Setup(HeroData character)
        {
            characterInfos = character;
            OnSetup(character.CharacterData);
        }

        protected override void OnDie()
        {
            BattleSiteManager.Instance.RemoveHero(transform);
        }
    }
}