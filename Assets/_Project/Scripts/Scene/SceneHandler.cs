using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using FTKingdom.Utils;

namespace FTKingdom
{
    public enum GameScene
    {
        InitialLoad,
        GameLoading,
        MainMenu,
        Gameplay
    }

    public class SceneHandler : LocalSingleton<SceneHandler>
    {
        private const string loadingSceneName = "GameLoading";
        private const float minimumLoadingTime = 1f;
        private Action<GameScene> s_onLoadScene;

        public void RegisterSceneChanged(Action<GameScene> callback)
        {
            if (callback != null)
            {
                s_onLoadScene += callback;
            }
        }

        public void RemoveSceneChanged(Action<GameScene> callback)
        {
            if (callback != null)
            {
                s_onLoadScene -= callback;
            }
        }

        public void LoadScene(GameScene gameScene)
        {
            LoadScene(gameScene.ToString());
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        // TODO: Think about loading percentage
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            LoadGameLoadingScene();

            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loadSceneOperation.allowSceneActivation = false;

            float loadingStartTime = Time.time;
            while (loadSceneOperation.progress < 0.9f || minimumLoadingTime > Time.time - loadingStartTime)
            {
                yield return null;
            }

            loadSceneOperation.allowSceneActivation = true;

            SetActiveScene(sceneName);
            SceneManager.UnloadSceneAsync(loadingSceneName);
        }

        private void LoadGameLoadingScene()
        {
            SceneManager.LoadScene(loadingSceneName);
            OnSceneLoad(loadingSceneName);
        }

        private void SetActiveScene(string sceneName)
        {
            SceneManager.SetActiveScene(GetSceneFromName(sceneName));
            OnSceneLoad(sceneName);
        }

        private Scene GetSceneFromName(string name)
        {
            return SceneManager.GetSceneByName(name);
        }

        private void OnSceneLoad(string sceneName)
        {
            if (Enum.TryParse(sceneName, out GameScene gameScene))
            {
                s_onLoadScene?.Invoke(gameScene);
            }
            else
            {
                LogHandler.SceneLog($"Scene {sceneName} is no a `GameScene`.");
            }
        }
    }
}