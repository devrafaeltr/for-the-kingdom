
using FTKingdom.Utils;

namespace FTKingdom
{
    public class CharacterStateWaiting : IState
    {
        internal override CharacterState StateType => CharacterState.Waiting;
        private CharacterBattle character;

        internal override void End(CharacterBattle entity)
        {
            EventsManager.RemoveListener(EventsManager.OnBattleStart, StartBattle);
        }

        internal override void Start(CharacterBattle entity)
        {
            EventsManager.AddListener(EventsManager.OnBattleStart, StartBattle);
            character = entity;
        }

        internal override void Update(CharacterBattle entity)
        { }

        private void StartBattle(IGameEvent gameEvent)
        {
            character.MoveTowardsTarget();
        }
    }
}