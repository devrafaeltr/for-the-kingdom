using System.Collections.Generic;
using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "New Storyline", menuName = "Dialogue/Storyline")]
    public class DialogueStoryline : ScriptableObject
    {
        [SerializeField] private List<DialogueLine> dialogueLines = new();
        public List<DialogueLine> DialogueLines => dialogueLines;
    }
}