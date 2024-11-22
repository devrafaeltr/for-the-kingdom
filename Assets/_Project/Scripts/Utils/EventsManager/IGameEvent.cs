using FTKingdom.UI;

namespace FTKingdom
{
    public interface IGameEvent { }

    public class UpdatePartyEvent : IGameEvent
    {
        public CharacterUIContainer NewMember { get; private set; }
        public CharacterUIContainer OldMember { get; private set; }
        public UIPartySlot Slot { get; private set; }

        public UpdatePartyEvent(UIPartySlot slot, CharacterUIContainer newMember, CharacterUIContainer oldMember = null)
        {
            NewMember = newMember;
            OldMember = oldMember;
            Slot = slot;
        }
    }
}