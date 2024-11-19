using UnityEngine;

namespace FTKingdom
{
    public class SkillSO : ScriptableObject
    {
        [Header("Basic infos")]
        [SerializeField] private new string name = "";
        [TextArea][SerializeField] private string description = "";
        [SerializeField] private Sprite graphic = null;

        public string Name => name;
        public string Description => description;
        public Sprite Graphic => graphic;
    }
}