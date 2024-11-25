using UnityEngine;

namespace FTKingdom.UI
{
    public class CampingSiteCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject partyContainter = null;

        // TODO: Check about party preview in the footer menu
        public void OpenPartyPanel()
        {
            partyContainter.SetActive(true);
        }

        public void GoToMap()
        {
            SceneHandler.Instance.LoadScene(GameScene.BattleSite);
        }
    }
}