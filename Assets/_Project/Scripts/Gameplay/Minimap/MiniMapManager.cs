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
            if (GameManager.Instance.LastLevelCompleted >= 0)
            {
                for (int i = 0; i < levels.Count; i++)
                {
                    if (i <= GameManager.Instance.LastLevelCompleted)
                    {
                        levels[i].InitLevel(i, LevelState.Completed);
                        continue;
                    }

                    levels[i].InitLevel(i, LevelState.Blocked);
                }
            }

            int nextLevel = GameManager.Instance.LastLevelCompleted + 1;
            if (levels.Count >= nextLevel)
            {
                levels[nextLevel].UnlockLevel();
            }
        }
    }
}