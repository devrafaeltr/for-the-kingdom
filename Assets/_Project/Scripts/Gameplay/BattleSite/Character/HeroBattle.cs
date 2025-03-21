namespace FTKingdom
{
    public class HeroBattle : CharacterBattle
    {
        protected HeroData characterInfos = null;

        public void Setup(HeroData character)
        {
            characterInfos = character;
            OnSetup(ScriptableDatabase.Instance.GetCharacterData(character.characterClass));
        }

        public void AddGold(int gold)
        {
            if (currenState == CharacterState.Dead)
            {
                return;
            }

            characterInfos.AddGold(gold);
        }

        public void AddExperience(int experience)
        {
            if (currenState == CharacterState.Dead)
            {
                return;
            }

            characterInfos.AddExperience(experience);
        }

        protected override void OnDie()
        {
            BattleSiteManager.Instance.RemoveHero(this);
        }
    }
}