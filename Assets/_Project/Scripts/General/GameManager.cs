using System.Collections.Generic;
using FTKingdom.Utils;

namespace FTKingdom
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public List<HeroData> CurrentHeroes => CurrentData.currentHeroes;

        public int LastLevelCompleted
        {
            get => CurrentData.lastLevelCompleted;
            private set => CurrentData.lastLevelCompleted = value;
        }

        private KingdomData CurrentData = new();
        public int currentLevel = 0;

        protected override void Awake()
        {
            base.Awake();
            GetSaved();

            LastLevelCompleted = -1;

            SceneHandler.Instance.LoadScene(GameScene.MainMenu);
        }

        private void OnDisable()
        {
            SaveData();
        }

        public List<HeroData> GetParty()
        {
            return CurrentData.currentHeroes.FindAll(c => c.IsOnParty);
        }

        public void SetCurrentLevelPlaying(int levelId)
        {
            currentLevel = levelId;
        }

        public int GetCurrentLevelPlaying()
        {
            return currentLevel;
        }

        public void UpdateLastCompletedLevel()
        {
            LastLevelCompleted = currentLevel;
            SaveData();
        }

        private void GetSaved()
        {
            CurrentData = SaveManager.LoadData<KingdomData>(SaveManager.kingdomData);
        }

        private void SaveData()
        {
            SaveManager.SaveData(SaveManager.kingdomData, CurrentData);
        }
    }
}