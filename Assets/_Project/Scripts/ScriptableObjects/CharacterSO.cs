using System.Collections.Generic;
using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Gameplay/Character")]
    public class CharacterSO : ScriptableObject
    {
        [Header("Basic infos")]
        [SerializeField] private new string name = "";
        [TextArea][SerializeField] private string description = "";
        [SerializeField] private Sprite graphic = null;
        [SerializeField] private CharacterType type;

        [Header("Stats infos")]
        [SerializeField] private int baseHp = 5;

        [Header("Combat infos")]
        [SerializeField] private ProjectileSO projectileData;
        [SerializeField] private int baseDamage = 5;
        [SerializeField] private float baseAttackDistance = 5;
        [SerializeField] private float baseAttackInterval = 5;
        [SerializeField] private int baseEvasionRate = 5;
        [SerializeField] private int baseCriticalRate = 5;
        [SerializeField] private float baseDefense = 5;
        [SerializeField] private List<BaseSpellSO> possibleSpells = new();

        public string Name => name;
        public string Description => description;
        public Sprite Graphic => graphic;
        public CharacterType Type => type;
        public CharacterAttackType AttackType
        {
            get => baseAttackDistance <= 2 ? CharacterAttackType.Melee : CharacterAttackType.Ranged;
        }
        public int BaseHp => baseHp;
        public ProjectileSO ProjectileData => projectileData;
        public int BaseDamage => baseDamage;
        public float BaseAttackDistance => baseAttackDistance;
        public float BaseAttackInterval => baseAttackInterval;
        public int BaseEvasionRate => baseEvasionRate;
        public int BaseCriticalRate => baseCriticalRate;
        public float BaseDefense => baseDefense;
        public List<BaseSpellSO> PossibleSpells => possibleSpells;
    }
}