using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "Gameplay/Spell")]
    public class BaseSpellSO : ScriptableObject
    {
        [Header("Basic info")]
        [SerializeField] private string spellName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;

        [Header("Behavior")]
        [SerializeField] private SpellBehaviorType behaviorType;
        [SerializeField] private SpellTrigger trigger;
        [SerializeField] private SpellType type;
        [SerializeField] private float cooldown;

        [Header("Projectile")]
        [SerializeField] private ProjectileSO projectileData;

        public string Name => spellName;
        public string Description => description;
        public Sprite Icon => icon;
        public SpellBehaviorType BehaviorType => behaviorType;
        public SpellType Type => type;
        public float Cooldown => cooldown;
        public SpellTrigger Trigger => trigger;
        public ProjectileSO ProjectileData => projectileData;
    }
}