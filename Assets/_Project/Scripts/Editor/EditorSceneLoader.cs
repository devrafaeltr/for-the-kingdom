using UnityEditor;
using UnityEditor.SceneManagement;

namespace FTKingdom
{
    public static class EditorSceneLoader
    {
        [MenuItem("Scenes/InitialLoad", priority = 0)]
        public static void LoadInitialLoad()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/General/{GameScene.InitialLoad}.unity");
        }

        [MenuItem("Scenes/MainMenu", priority = 1)]
        public static void LoadSceneMenu()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/Gameplay/{GameScene.MainMenu}.unity");
        }

        [MenuItem("Scenes/CampingSite", priority = 2)]
        public static void LoadCampingSite()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/Gameplay/{GameScene.CampingSite}.unity");
        }

        [MenuItem("Scenes/MiniMap", priority = 3)]
        public static void LoadMiniMap()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/Gameplay/{GameScene.MiniMap}.unity");
        }

        [MenuItem("Scenes/BattleSite", priority = 4)]
        public static void LoadBattleSite()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/Gameplay/{GameScene.BattleSite}.unity");
        }
    }
}