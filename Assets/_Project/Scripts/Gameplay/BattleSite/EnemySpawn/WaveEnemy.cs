using UnityEngine;

namespace FTKingdom
{
    [System.Serializable]
    public class WaveEnemy
    {
        [SerializeField] private CharacterSO enemy;
        [SerializeField] private int position;

        public CharacterSO Enemy => enemy;
        public int Position => position;
    }
}
