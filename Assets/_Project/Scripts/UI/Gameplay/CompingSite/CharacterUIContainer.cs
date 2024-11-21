using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FTKingdom
{
    [System.Serializable]
    public class CharacterUIContainer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image imgCharacter = null;

        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private float canvasScale = 1f;
        [HideInInspector] public Character character;

        public void Setup(Character newCharacter, float scaleFactor)
        {
            character = newCharacter;
            ConfigureElement(scaleFactor);
        }

        public void FitToParent()
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        private void ConfigureElement(float scaleFactor)
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasScale = scaleFactor;

            imgCharacter.sprite = character.CharacterData.Graphic;
        }

        public void ChangePartySlot(int slot)
        {
            character.SetPartySlot(slot);
        }

        public void RemoveFromParty()
        {
            character.SetPartySlot(-1);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false; // Disable raycast blocking to allow detecting drop targets
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvasScale;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true; // Re-enable raycast blocking
            if (eventData.pointerEnter == null || !eventData.pointerEnter.CompareTag("PartySlot"))
            {
                FitToParent();
            }
        }
    }
}