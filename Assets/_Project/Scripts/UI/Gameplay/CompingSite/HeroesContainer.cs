using UnityEngine;
using UnityEngine.EventSystems;

namespace FTKingdom.UI
{
    public class HeroesContainer : MonoBehaviour, IDropHandler
    {
        // TODO: Merge wih UIPartySlot
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.CompareTag("CharacterUI"))
            {
                PartyContainerController.Instance.RemoveFromParty(eventData.pointerDrag.GetComponent<CharacterUIContainer>());
            }
        }
    }
}