using UnityEditor;
using UnityEditor.SceneManagement;

namespace FTKingdom
{
    public class EditorSceneLoader
    {

        [MenuItem("Scenes/InitialLoad")]
        public static void LoadInitialLoad()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/General/{GameScene.InitialLoad}.unity");
        }

        [MenuItem("Scenes/MainMenu")]
        public static void LoadSceneMenu()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/Gameplay/{GameScene.MainMenu}.unity");
        }

        [MenuItem("Scenes/CampingSite")]
        public static void LoadCampingSite()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/Gameplay/{GameScene.CampingSite}.unity");
        }

        [MenuItem("Scenes/MiniMap")]
        public static void LoadMiniMap()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/Gameplay/{GameScene.MiniMap}.unity");
        }

        [MenuItem("Scenes/BattleSite")]
        public static void LoadBattleSite()
        {
            EditorSceneManager.OpenScene($"Assets/_Project/Scenes/Gameplay/{GameScene.BattleSite}.unity");
        }
    }
}