using UnityEngine;

namespace FTKingdom.UI
{
    public class MiniMapCanvas : MonoBehaviour
    {
        public void StartCombat()
        {
            // TODO: Assign the actual level when add level/minimap system.
            GameManager.Instance.LevelPlaying = 0;

            // TODO: Create scenes for each battle site and load the right one.
            // TODO: Well, actually... Maybe create new scenes only for different terrains/areas.
            // But certanly not for each battle site.
            SceneHandler.Instance.LoadScene(GameScene.BattleSite);
        }

        public void BackToCampingSite()
        {
            SceneHandler.Instance.LoadScene(GameScene.CampingSite);
        }
    }
}