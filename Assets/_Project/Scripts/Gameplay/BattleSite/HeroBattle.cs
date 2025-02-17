namespace FTKingdom
{
    public class HeroBattle : CharacterBattle
    {
        protected Character characterInfos = null;

        public void Setup(Character character)
        {
            characterInfos = character;
            characterData = character.CharacterData;
            spriteRenderer.sprite = character.CharacterData.Graphic;

            OnSetup();
        }
    }
}