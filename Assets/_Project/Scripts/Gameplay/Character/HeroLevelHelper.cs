using System.Collections.Generic;

namespace FTKingdom
{
    public static class HeroLevelHelper
    {
        public static readonly int MaxLevel = 10;

        // TODO: Maybe move this to a ScriptableObject.
        private static readonly Dictionary<HeroRank, int[]> rankToLevelsExp = new()
        {
            { HeroRank.E, new int[] { 10, 25, 50, 90, 140, 200, 280, 380, 500, 650 } },
            { HeroRank.D, new int[] { 20, 40, 70, 120, 180, 250, 340, 450, 580, 750 } },
            { HeroRank.C, new int[] { 30, 60, 100, 160, 230, 320, 430, 560, 710, 900 } },
            { HeroRank.B, new int[] { 40, 80, 130, 210, 300, 410, 540, 690, 860, 1100 } },
            { HeroRank.A, new int[] { 50, 100, 160, 250, 350, 470, 610, 770, 950, 1200 } },
            { HeroRank.S, new int[] { 60, 120, 190, 290, 400, 530, 680, 850, 1040, 1300 } }
        };

        public static int GetMaxExperienceByLevel(HeroRank rank, int level)
        {
            return rankToLevelsExp[rank][level - 1];
        }
    }
}