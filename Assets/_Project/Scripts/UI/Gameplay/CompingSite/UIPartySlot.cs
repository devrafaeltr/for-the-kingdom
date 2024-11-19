using FTKingdom.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FTKingdom.UI
{
    public class UIPartySlot : MonoBehaviour, IDropHandler
    {
        private CharacterUIContainer currentMember = null;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.CompareTag("CharacterUI"))
            {
                var newMember = eventData.pointerDrag.GetComponent<CharacterUIContainer>();
                UpdatePartyEvent updateEvent = new(currentMember, newMember);
                EventsManager.Publish(EventsManager.OnChangePartyMember, updateEvent);

                if (currentMember != null)
                {
                    currentMember.transform.SetParent(newMember.transform.parent);
                    currentMember.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }

                newMember.transform.SetParent(transform);
                newMember.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Reset position

                currentMember = newMember;
            }
        }
    }
}