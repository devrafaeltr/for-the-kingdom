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
            // TODO: Maybe add level id on inspector.
            for (int i = 0; i < levels.Count; i++)
            {
                if (i < GameManager.Instance.LastLevelCompleted)
                {
                    levels[i].InitLevel(i + 1, LevelState.Completed);
                    continue;
                }

                levels[i].InitLevel(i + 1, LevelState.Blocked);
            }

            int nextLevelIndex = GameManager.Instance.LastLevelCompleted;
            if (levels.Count > nextLevelIndex)
            {
                levels[nextLevelIndex].UnlockLevel();
            }
        }
    }
}