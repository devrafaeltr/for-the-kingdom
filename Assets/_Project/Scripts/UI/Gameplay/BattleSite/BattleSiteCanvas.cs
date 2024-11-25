using UnityEngine;

namespace FTKingdom.UI
{
    public class BattleSiteCanvas : MonoBehaviour
    {
        public void StartCombat()
        {
        }

        public void BackToMinimap()
        {
            SceneHandler.Instance.LoadScene(GameScene.MiniMap);
        }
    }
}