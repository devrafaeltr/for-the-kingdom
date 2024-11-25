using UnityEngine;

namespace FTKingdom.UI
{
    public class MiniMapCanvas : MonoBehaviour
    {
        public void StartCombat()
        {
            // TODO: Create scenes for each battle site and load the right one.
            SceneHandler.Instance.LoadScene(GameScene.BattleSite);
        }

        public void BackToCampingSite()
        {
            SceneHandler.Instance.LoadScene(GameScene.CampingSite);
        }
    }
}