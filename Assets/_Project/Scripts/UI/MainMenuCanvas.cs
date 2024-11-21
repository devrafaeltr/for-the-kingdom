using UnityEngine;

namespace FTKingdom.UI
{
    public class MainMenuCanvas : MonoBehaviour
    {
        // TODO: Move player to save/load, then play game (?)
        public void PlayGame()
        {
            SceneHandler.Instance.LoadScene(GameScene.CampingSite);
        }

        private void OpenSettings()
        {
            // Method intentionally left empty.
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}