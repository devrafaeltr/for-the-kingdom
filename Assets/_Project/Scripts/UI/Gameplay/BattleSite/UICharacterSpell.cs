using UnityEngine;
using UnityEngine.UI;

namespace FTKingdom
{
    public class UICharacterSpell : MonoBehaviour
    {
        [SerializeField] private Image imgSpellIcon;
        [SerializeField] private Image imgSpellCooldown;

        private CharacterSpell spell;

        private void Update()
        {
            float value = spell.currentCooldown / spell.spellData.Cooldown;
            imgSpellCooldown.fillAmount = Mathf.Clamp01(value);
        }

        public void Setup(CharacterSpell spell)
        {
            this.spell = spell;
            imgSpellIcon.sprite = spell.spellData.Icon;
        }
    }
}