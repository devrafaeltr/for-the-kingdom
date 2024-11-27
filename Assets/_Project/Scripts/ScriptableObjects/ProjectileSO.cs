using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
    public class ProjectileSO : ScriptableObject
    {
        [SerializeField] private Sprite graphic = null;
        [SerializeField] private float speed = 10f;
        [SerializeField] private int damage = 10;

        public Sprite Graphic => graphic;
        public float Speed => speed;
        public int Damage => damage;
    }
}