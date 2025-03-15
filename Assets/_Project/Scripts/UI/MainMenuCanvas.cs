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

        public void OpenSettings()
        {
            // TODO: Open settings
            SceneHandler.Instance.LoadScene(GameScene.Introduction);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}