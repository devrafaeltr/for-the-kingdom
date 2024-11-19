namespace FTKingdom
{
    public interface IGameEvent { }

    public class UpdatePartyEvent : IGameEvent
    {
        public CharacterUIContainer OldMember { get; private set; }
        public CharacterUIContainer NewMember { get; private set; }

        public UpdatePartyEvent(CharacterUIContainer oldMember, CharacterUIContainer newMember)
        {
            OldMember = oldMember;
            NewMember = newMember;
        }
    }
}