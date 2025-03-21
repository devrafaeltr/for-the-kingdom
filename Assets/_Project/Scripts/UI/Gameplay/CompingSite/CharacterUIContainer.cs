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
        private HeroData character;
        private float canvasScale = 1f;

        public bool IsOnParty => character.partySlot >= 0;

        public void Setup(HeroData newCharacter, float scaleFactor)
        {
            character = newCharacter;
            ConfigureElement(scaleFactor);
        }

        private void ConfigureElement(float scaleFactor)
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvasScale = scaleFactor;

            Sprite sprite = ScriptableDatabase.Instance.GetCharacterData(character.characterClass).Graphic;
            imgCharacter.sprite = sprite;
        }

        public void FitToParent()
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        public void ChangePartySlot(int slotIndex)
        {
            character.partySlot = slotIndex;
            FitToParent();
        }

        public void RemoveFromParty()
        {
            character.partySlot = -1;
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