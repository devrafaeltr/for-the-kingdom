namespace FTKingdom
{
    [System.Serializable]
    public class Character
    {
        public CharacterSO CharacterData { get; private set; }
        public int CurrentLevel { get; private set; }
        public bool IsOnParty { get; private set; } = false;
        public int PartySlot { get; private set; } = -1;

        public void SetCharacterData(CharacterSO data)
        {
            CharacterData = data;
        }

        public void DoLevelUp()
        {
            CurrentLevel++;
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