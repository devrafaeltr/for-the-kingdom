namespace FTKingdom
{
    public class Character
    {
        public CharacterSO CharacterData { get; private set; }
        public int CurrentLevel { get; private set; }
        public bool IsOnParty { get; private set; }

        public void SetData(CharacterSO data)
        {
            CharacterData = data;
        }

        public void DoLevelUp()
        {
            CurrentLevel++;
        }

        public void ChangePartyStaus(bool status)
        {
            IsOnParty = status;
        }
    }
}