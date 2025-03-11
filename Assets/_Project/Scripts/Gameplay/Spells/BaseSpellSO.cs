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
        [SerializeField] private float cooldown;
        [SerializeField] private ProjectileSO projectileData;
        [SerializeField] private SpellTrigger trigger;

        public string Name => spellName;
        public string Description => description;
        public Sprite Icon => icon;
        public float Cooldown => cooldown;
        public ProjectileSO ProjectileData => projectileData;
        public SpellTrigger Trigger => trigger;
    }
}