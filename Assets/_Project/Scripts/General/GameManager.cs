using System.Collections.Generic;
using FTKingdom.Utils;

namespace FTKingdom
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        private List<Character> currentCharacters = new();
        protected override void Awake()
        {
            base.Awake();
            SceneHandler.Instance.LoadScene(GameScene.MainMenu);
        }

        public List<Character> GetParty()
        {
            return currentCharacters.FindAll(c => c.IsOnParty);
        }
    }
}