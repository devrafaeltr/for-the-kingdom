namespace FTKingdom
{
    public class HeroBattle : CharacterBattle
    {
        protected HeroData characterInfos = null;

        public void Setup(HeroData character)
        {
            characterInfos = character;
            OnSetup(ScriptableDatabase.Instance.GetCharacterData(character.CharacterClass));
        }

        protected override void OnDie()
        {
            BattleSiteManager.Instance.RemoveHero(this);
        }
    }
}