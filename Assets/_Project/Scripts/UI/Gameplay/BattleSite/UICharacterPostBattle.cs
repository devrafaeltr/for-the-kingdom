using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTKingdom
{
    public class UICharacterPostBattle : MonoBehaviour
    {
        [SerializeField] private Image heroPortrait = null;
        [Header("Damage")]
        [SerializeField] private TextMeshProUGUI txtHeroDamageDealt = null;
        [SerializeField] private TextMeshProUGUI txtHeroSelfDamange = null;
        [Header("Support")]
        [SerializeField] private TextMeshProUGUI txtHeroHealing = null;
        [Header("Resource")]
        [SerializeField] private TextMeshProUGUI txtHeroExperience = null;
        [SerializeField] private TextMeshProUGUI txtHeroGold = null;

        // TODO: Pass hero(?)
        public void Setup()
        {
            txtHeroGold.text = $"Gold: {BattleSiteManager.Instance.GetGoldReward()}";
            txtHeroExperience.text = $"Exp: {BattleSiteManager.Instance.GetExperienceReward()}";

            // TODO: Add exp and gold do hero
        }
    }
}