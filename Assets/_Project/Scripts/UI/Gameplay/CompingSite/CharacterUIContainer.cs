using FTKingdom.UI;
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
        private Character character;
        private float canvasScale = 1f;

        public UIPartySlot CurrentSlot { get; private set; }
        public bool IsOnParty => character.PartySlot >= 0;

        public void Setup(Character newCharacter, float scaleFactor)
        {
            character = newCharacter;
            ConfigureElement(scaleFactor);
        }

        private void ConfigureElement(float scaleFactor)
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasScale = scaleFactor;

            imgCharacter.sprite = character.CharacterData.Graphic;
        }

        public void FitToParent()
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        public void ChangePartySlot(int slotIndex, UIPartySlot slot)
        {
            // if (slotIndex == -1 || CurrentSlot != null)
            // {
            //     CurrentSlot.RemoveMember();
            // }

            CurrentSlot = slot;
            CurrentSlot.SetCurrentMember(this);

            character.SetPartySlot(slotIndex);
            transform.SetParent(slot.transform);

            FitToParent();
        }

        public void RemoveFromParty()
        {
            if (character.IsOnParty)
            {
                CurrentSlot.RemoveMember();
                CurrentSlot = null;
                character.SetPartySlot(-1);
            }

            FitToParent();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvasScale;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            if (eventData.pointerEnter != null)
            {
                GameObject ladindObj = eventData.pointerEnter;
                if (ladindObj.CompareTag("HeroesContainer"))
                {
                    PartyContainerController.Instance.RemoveFromParty(this);
                }
                else if (!ladindObj.CompareTag("PartySlot"))
                {
                    FitToParent();
                }
            }
            else
            {
                FitToParent();
            }
        }

        private void OnDisable()
        {
            GenericPool.ReleaseItem(this, PoolType.CharacerUI);
        }
    }
}