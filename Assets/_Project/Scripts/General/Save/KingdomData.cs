using System.Collections.Generic;

namespace FTKingdom
{
    [System.Serializable]
    public class KingdomData
    {
        public int lastLevelCompleted = 0;
        public List<HeroData> currentHeroes = new();
    }
}