using UnityEngine;

namespace FTKingdom.UI
{
    public class GameplayMenuCanvas : MonoBehaviour
    {
        [Header("Gameplay setup")]
        [Header("GameOver setup")]
        [SerializeField] private GameObject gameOverPanel = null;

        [Header("Pause setup")]
        [SerializeField] private GameObject pausePanel = null;

        #region Gameplay
        #endregion

        #region GameOver
        private void OnGameOver()
        {
            gameOverPanel.SetActive(true);
        }

        public void Restart()
        {
            SceneHandler.Instance.LoadScene(GameScene.Gameplay);
        }

        public void BackToMenu()
        {
            SceneHandler.Instance.LoadScene(GameScene.MainMenu);
        }
        #endregion GameOver

        #region Pause
        private void OnPause()
        {
            pausePanel.SetActive(true);
        }

        private void OnResume()
        {
            pausePanel.SetActive(false);
        }

        public void ResumeGame()
        {
            OnResume();
        }
        #endregion

        private void OnDisable()
        {

        }
    }
}