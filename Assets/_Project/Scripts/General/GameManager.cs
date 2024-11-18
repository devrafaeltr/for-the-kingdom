using FTKingdom.Utils;

namespace FTKingdom
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();
            SceneHandler.Instance.LoadScene(GameScene.MainMenu);
        }
    }
}