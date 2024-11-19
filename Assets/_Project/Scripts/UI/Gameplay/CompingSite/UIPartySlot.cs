using UnityEngine;
using UnityEngine.EventSystems;

namespace FTKingdom.UI
{
    public class UIPartySlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            GameObject draggable = eventData.pointerDrag;
            if (eventData.pointerDrag != null && eventData.pointerDrag.CompareTag("CharacterUI"))
            {
                draggable.transform.SetParent(transform);
                draggable.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Reset position
            }
        }
    }
}