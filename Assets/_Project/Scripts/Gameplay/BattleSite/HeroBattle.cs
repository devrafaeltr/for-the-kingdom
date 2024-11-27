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

            FindEnemy();
            OnSetup();
        }

        protected override void OnSetup()
        {
            // TODO: Maybe get from Character class instead, with modifiers
            navMeshAgent.stoppingDistance = characterInfos.CharacterData.BaseAttackDistance;
        }
    }
}