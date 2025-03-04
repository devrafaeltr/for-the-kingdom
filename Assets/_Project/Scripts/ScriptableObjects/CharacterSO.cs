using System.Collections.Generic;
using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Character")]
    public class CharacterSO : ScriptableObject
    {
        [Header("Basic infos")]
        [SerializeField] private new string name = "";
        [TextArea][SerializeField] private string description = "";
        [SerializeField] private Sprite graphic = null;
        [SerializeField] private CharacterType type;

        [Header("Stats infos")]
        [SerializeField] private int baseHp = 5;
        [SerializeField] private int baseMp = 0;
        [SerializeField] private List<SkillSO> possibleSkills = new();

        [Header("Combat infos")]
        [SerializeField] private int baseDamage = 5;
        [SerializeField] private float baseAttackDistance = 5;
        [SerializeField] private float baseAttackInterval = 5;


        public string Name => name;
        public string Description => description;
        public Sprite Graphic => graphic;
        public CharacterType Type => type;
        public CharacterAttackType AttackType
        {
            get => baseAttackDistance <= 1 ? CharacterAttackType.Melee : CharacterAttackType.Ranged;
        }
        public int BaseHp => baseHp;
        public int BaseMp => baseMp;
        public List<SkillSO> PossibleSkills => possibleSkills;
        public int BaseDamage => baseDamage;
        public float BaseAttackDistance => baseAttackDistance;
        public float BaseAttackInterval => baseAttackInterval;
    }
}