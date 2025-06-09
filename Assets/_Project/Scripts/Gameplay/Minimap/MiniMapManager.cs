using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class MiniMapManager : LocalSingleton<MiniMapManager>
    {
        [SerializeField] private List<MiniMapLevel> levels = new();

        private void Awake()
        {
            SetupLevels();
        }

        private void SetupLevels()
        {
            int currentLevel = GameManager.Instance.LastLevelCompleted;

            // TODO: Maybe add level id on inspector.
            for (int i = 0; i < currentLevel; i++)
            {
                levels[i].InitLevel(i + 1, LevelState.Completed);
            }

            if (currentLevel < levels.Count)
            {
                levels[currentLevel].UnlockLevel(currentLevel + 1);
            }
        }
    }
}