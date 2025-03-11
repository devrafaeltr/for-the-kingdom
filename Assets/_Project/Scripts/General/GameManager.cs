using System;
using System.Collections.Generic;
using FTKingdom.Utils;
using Random = UnityEngine.Random;

namespace FTKingdom
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public List<HeroData> CurrentHeroes => CurrentData.currentHeroes;

        public int LevelPlaying
        {
            get => CurrentData.lastLevelCompleted;
        }

        private KingdomData CurrentData = new();

        protected override void Awake()
        {
            base.Awake();
            GetSaved();

            SceneHandler.Instance.LoadScene(GameScene.MainMenu);
        }

        public List<HeroData> GetParty()
        {
            return CurrentData.currentHeroes.FindAll(c => c.IsOnParty);
        }

        public void SetCurrentLevelPlaying(int levelId)
        {

        }

        private void GetSaved()
        {
            CurrentData = SaveManager.LoadData<KingdomData>(SaveManager.kingdomData);
        }
    }
}