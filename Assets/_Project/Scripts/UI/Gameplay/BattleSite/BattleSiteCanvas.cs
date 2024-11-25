using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom.UI
{
    public class BattleSiteCanvas : MonoBehaviour
    {
        public void StartCombat()
        {
            EventsManager.Publish(EventsManager.OnBattleStart);
        }

        public void BackToMinimap()
        {
            SceneHandler.Instance.LoadScene(GameScene.MiniMap);
        }
    }
}