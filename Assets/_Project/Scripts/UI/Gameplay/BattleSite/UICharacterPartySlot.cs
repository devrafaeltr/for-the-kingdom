using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTKingdom
{
    public class UICharacterPartySlot : MonoBehaviour
    {
        [SerializeField] private CanvasGroup chacterGroup = null;

        [Header("Character infos")]
        [SerializeField] private Image imgPortrait;
        [SerializeField] private TextMeshProUGUI txtCharacterName;

        [Header("Health")]
        [SerializeField] private Image imgHealth;
        [SerializeField] private TextMeshProUGUI txtHealth;
        [SerializeField] private Image deadImage;

        [Header("Spells")]
        [SerializeField] private Transform spellsContainer;
        [SerializeField] private UICharacterSpell spellPrefab;

        private int maxHp;

        public void Setup(CharacterBattle character, int currentHp)
        {
            imgPortrait.sprite = character.CharacterData.Graphic;
            txtCharacterName.text = character.CharacterData.Name;

            maxHp = currentHp;

            txtHealth.text = $"{maxHp}/{maxHp}";

            chacterGroup.alpha = 1f;

            SetupSpells(character.CharacterSpells);
        }

        public void UpdateHealth(int currentHp)
        {
            txtHealth.text = $"{currentHp}/{maxHp}";
            DoImageFill(currentHp);

            if (currentHp <= 0)
            {
                deadImage.enabled = true;
            }
        }

        private void DoImageFill(float current)
        {
            imgHealth.DOFillAmount(current / maxHp, 0.2f);
        }

        private void SetupSpells(List<CharacterSpell> characterSpells)
        {
            foreach (var spell in characterSpells)
            {
                var spellInstance = Instantiate(spellPrefab, spellsContainer);
                spellInstance.Setup(spell);
            }
        }
    }
}