using UnityEditor;
using UnityEditor.SceneManagement;

namespace FTKingdom
{
    public static class EditorSceneLoader
    {
        [MenuItem("Scenes/InitialLoad", priority = 0)]
        public static void LoadInitialLoad()
        {
            OpenEditorScene($"Assets/_Project/Scenes/General/{GameScene.InitialLoad}");
        }

        [MenuItem("Scenes/MainMenu", priority = 1)]
        public static void LoadSceneMenu()
        {
            OpenEditorScene($"Assets/_Project/Scenes/General/{GameScene.MainMenu}");
        }

        [MenuItem("Scenes/CampingSite", priority = 2)]
        public static void LoadCampingSite()
        {
            OpenEditorScene($"Assets/_Project/Scenes/Gameplay/{GameScene.CampingSite}");
        }

        [MenuItem("Scenes/MiniMap", priority = 3)]
        public static void LoadMiniMap()
        {
            OpenEditorScene($"Assets/_Project/Scenes/Gameplay/{GameScene.MiniMap}");
        }

        [MenuItem("Scenes/BattleSite", priority = 4)]
        public static void LoadBattleSite()
        {
            OpenEditorScene($"Assets/_Project/Scenes/Gameplay/{GameScene.BattleSite}");
        }

        private static void OpenEditorScene(string scenePath)
        {
            EditorSceneManager.OpenScene($"{scenePath}.unity");
        }
    }
}