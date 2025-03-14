using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class BattleSiteCanvas : LocalSingleton<BattleSiteCanvas>
    {
        [SerializeField] private List<UICharacterPartySlot> heroes = new();
        [SerializeField] private List<UICharacterPartySlot> enemies = new();

        public void AddHeroToParty(CharacterBattle hero, int slotPosition)
        {
            heroes[slotPosition].Setup(hero, hero.GetHealth());
        }

        public void AddEnemyToParty(CharacterBattle enemy, int slotPosition)
        {
            enemies[slotPosition].Setup(enemy, enemy.GetHealth());
        }

        public void UpdateHealth(CharacterBattle character)
        {
            if (character.CharacterData.Type == CharacterType.Hero)
            {
                heroes[character.CharacterPosition].UpdateHealth(character.GetHealth());
            }
            else
            {
                enemies[character.CharacterPosition].UpdateHealth(character.GetHealth());
            }
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