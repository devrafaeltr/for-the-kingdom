using System.Collections.Generic;
using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Character", order = 0)]
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

        public string Name => name;
        public string Description => description;
        public Sprite Graphic => graphic;
        public CharacterType Type => type;
        public int BaseHp => baseHp;
        public int BaseMp => baseMp;
        public List<SkillSO> PossibleSkills => possibleSkills;
    }
}