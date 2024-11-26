using FTKingdom.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FTKingdom.UI
{
    public class UIPartySlot : MonoBehaviour, IDropHandler
    {
        private CharacterUIContainer currentMember = null;

        public void SetCurrentMember(CharacterUIContainer newMember)
        {
            currentMember = newMember;
        }

        public void RemoveMember()
        {
            currentMember = null;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.CompareTag("CharacterUI"))
            {
                AssignMember(eventData.pointerDrag.GetComponent<CharacterUIContainer>());
            }
        }

        private void AssignMember(CharacterUIContainer newMember)
        {
            if (newMember == currentMember)
            {
                currentMember.FitToParent();
                return;
            }

            UpdatePartyEvent updateEvent = new(this, newMember, currentMember);
            EventsManager.Publish(EventsManager.OnChangePartyMember, updateEvent);
        }
    }
}