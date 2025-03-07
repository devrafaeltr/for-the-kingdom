using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "EnemyWave", menuName = "Gameplay/EnemyWave")]
    public class EnemyWave : ScriptableObject
    {
        [SerializeField] private WaveEnemy[] enemies;
        public WaveEnemy[] Enemies => enemies;
    }
}
