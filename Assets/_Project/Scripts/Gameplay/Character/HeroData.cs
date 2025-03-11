using System.Collections.Generic;

namespace FTKingdom
{
    [System.Serializable]
    public class HeroData
    {
        public ClassType CharacterClass;
        public int CurrentLevel;
        public bool IsOnParty = false;
        public int PartySlot = -1;
        public List<SpellType> HeroSpells = new();

        public void SetCharacterClass(ClassType type)
        {
            CharacterClass = type;
        }

        public void SetPartySlot(int slot)
        {
            PartySlot = slot;

            if (slot >= 0)
            {
                IsOnParty = true;
            }
            else
            {
                IsOnParty = false;
            }
        }
    }
}