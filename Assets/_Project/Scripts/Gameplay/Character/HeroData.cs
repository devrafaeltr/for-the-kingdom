using System.Collections.Generic;

namespace FTKingdom
{
    [System.Serializable]
    public class HeroData
    {
        public HeroRank currentRank = HeroRank.E;
        public bool CanUpgradeRank
        {
            get
            {
                return currentExperience == MaxExperience && currentRank != HeroRank.S;
            }
        }
        public ClassType characterClass;
        public int partySlot = -1;
        public bool IsOnParty => partySlot >= 0;

        public int currentLevel = 1;
        public int currentExperience = 0;
        private int MaxExperience => HeroLevelHelper.GetMaxExperienceByLevel(currentRank, currentLevel);
        public int currentGold = 0;

        public List<SpellType> HeroSpells = new();

        public void SetCharacterClass(ClassType type)
        {
            characterClass = type;
        }

        public void AddGold(int gold)
        {
            currentGold += gold;
        }

        public void AddExperience(int experience)
        {
            if (currentLevel == HeroLevelHelper.MaxLevel)
            {
                return;
            }

            currentExperience += experience;

            if (currentExperience >= MaxExperience)
            {
                currentExperience = MaxExperience;
                OnLevelUp();
            }
        }

        public void UpgradeRank()
        {
            OnRankUp();
        }

        private void OnLevelUp()
        {
            currentExperience = 0;
            currentLevel++;
        }

        private void OnRankUp()
        {
            if (!CanUpgradeRank)
            {
                return;
            }

            currentExperience = 0;
            currentLevel = 1;
            currentRank = (HeroRank)((int)currentRank + 1);
        }
    }
}