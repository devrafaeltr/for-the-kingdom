using UnityEngine;
using UnityEngine.EventSystems;

namespace FTKingdom
{
    public class CharacterUIContainer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private float canvasScale = 1f;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();

            var parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                canvasScale = parentCanvas.scaleFactor;
            }
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
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}