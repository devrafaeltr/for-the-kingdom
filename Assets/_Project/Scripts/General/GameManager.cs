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
        private int currentLevel = 0;

        protected override void Awake()
        {
            base.Awake();
            GetSaved();

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

        public int GetCurrentLevel()
        {
            return currentLevel;
        }

        public int GetCurrentLevelAsIndex()
        {
            UnityEngine.Debug.Log($"GetCurrentLevelAsIndex: {currentLevel - 1}");
            return currentLevel - 1;
        }

        public void MarkCurrentLevelAsCompleted()
        {
            LastLevelCompleted = currentLevel;
            EventsManager.Publish(EventsManager.OnBattleEnd);
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