using FTKingdom.UI;
using UnityEngine;

namespace FTKingdom
{
    public interface IGameEvent { }

    public class UpdatePartyEvent : IGameEvent
    {
        public CharacterUIContainer NewMember { get; private set; }
        public CharacterUIContainer CurrentMember { get; private set; }
        public UIPartySlot Slot { get; private set; }

        public UpdatePartyEvent(UIPartySlot slot, CharacterUIContainer newMember, CharacterUIContainer currentMember)
        {
            NewMember = newMember;
            CurrentMember = currentMember;
            Slot = slot;
        }
    }

    public class OnCharacterDieEvent : IGameEvent
    {
        public Transform Character { get; private set; }

        public OnCharacterDieEvent(Transform character)
        {
            Character = character;
        }
    }
}