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

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.CompareTag("CharacterUI"))
            {
                var newMember = eventData.pointerDrag.GetComponent<CharacterUIContainer>();
                UpdatePartyEvent updateEvent = new(this, newMember, currentMember);

                currentMember = newMember;
                newMember.transform.SetParent(transform);

                EventsManager.Publish(EventsManager.OnChangePartyMember, updateEvent);
            }
        }
    }
}