using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "New Projectile", menuName = "Gameplay/Projectile")]
    public class ProjectileSO : ScriptableObject
    {
        [SerializeField] private GameObject projectilePrefab;

        [Header("Graphics")]
        [SerializeField] private Sprite graphic = null;
        [SerializeField] private AnimatorOverrideController animatorOverrider = null;

        [Header("Stats")]
        [SerializeField] private float speed = 10f;

        public GameObject ProjectilePrefab => projectilePrefab;
        public Sprite Graphic => graphic;
        public AnimatorOverrideController AnimatorOverrider => animatorOverrider;
        public float Speed => speed;
    }
}