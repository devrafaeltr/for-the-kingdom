using FTKingdom.Utils;

namespace FTKingdom
{
    public class HeroBattle : CharacterBattle
    {
        protected Character characterInfos = null;

        public void Setup(Character character)
        {
            characterInfos = character;
            CharacterData = character.CharacterData;
            spriteRenderer.sprite = character.CharacterData.Graphic;

            OnSetup();
        }

        protected override void OnDie()
        {
            BattleSiteManager.Instance.UpdateHeroes(transform);
        }
    }
}