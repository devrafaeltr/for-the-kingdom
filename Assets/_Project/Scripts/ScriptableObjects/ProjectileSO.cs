using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
    public class ProjectileSO : ScriptableObject
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Sprite graphic = null;
        [SerializeField] private float speed = 10f;

        public GameObject ProjectilePrefab => projectilePrefab;
        public Sprite Graphic => graphic;
        public float Speed => speed;
    }
}