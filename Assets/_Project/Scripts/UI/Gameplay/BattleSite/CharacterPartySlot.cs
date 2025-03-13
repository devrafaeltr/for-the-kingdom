using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTKingdom
{
    public class CharacterPartySlot : MonoBehaviour
    {
        [SerializeField] private CanvasGroup chacterGroup = null;

        [Header("Character infos")]
        [SerializeField] private Image imgPortrait;
        [SerializeField] private TextMeshProUGUI txtCharacterName;

        [Header("Health")]
        [SerializeField] private Image imgHealth;
        [SerializeField] private TextMeshProUGUI txtHealth;

        private int maxHp;

        public void Setup(CharacterSO characterData, int currentHp)
        {
            imgPortrait.sprite = characterData.Graphic;
            txtCharacterName.text = characterData.Name;

            maxHp = currentHp;

            txtHealth.text = $"{maxHp}/{maxHp}";

            chacterGroup.alpha = 1f;
        }

        public void UpdateHealth(int currentHp)
        {
            txtHealth.text = $"{currentHp}/{maxHp}";
        }
    }
}