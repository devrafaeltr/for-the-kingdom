using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class ScriptableDatabase : LocalSingleton<ScriptableDatabase>
    {
        [SerializeField] private List<ClassDataHolder> classToData = new();
        [SerializeField] private List<SpellDataHolder> spellToData = new();

        public CharacterSO GetCharacterData(ClassType type)
        {
            return classToData.Find(d => d.type == type).data;
        }

        public BaseSpellSO GetCharacterData(SpellType type)
        {
            return spellToData.Find(d => d.type == type).data;
        }
    }

    [System.Serializable]
    public class ClassDataHolder
    {
        public ClassType type;
        public CharacterSO data;
    }

    [System.Serializable]
    public class SpellDataHolder
    {
        public SpellType type;
        public BaseSpellSO data;
    }
}