using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom.UI
{
    public class BattleSiteCanvas : LocalSingleton<BattleSiteCanvas>
    {
        [SerializeField] private Transform heroPartyContainer = null;
        [SerializeField] private Transform enemyPartyContainer = null;

        [SerializeField] private List<CharacterPartySlot> heroes = new();
        [SerializeField] private List<CharacterPartySlot> enemies = new();

        public void AddHeroToParty(HeroBattle hero, int slotPosition)
        {
            heroes[slotPosition].Setup(hero.CharacterData, hero.GetHealth());
        }

        public void AddEnemyToParty(CharacterBattle enemy, int slotPosition)
        {
            enemies[slotPosition].Setup(enemy.CharacterData, enemy.GetHealth());
        }

        #region UI Buttons
        public void StartCombat()
        {
            EventsManager.Publish(EventsManager.OnBattleStart);
        }

        public void BackToMinimap()
        {
            SceneHandler.Instance.LoadScene(GameScene.MiniMap);
        }
        #endregion
    }
}