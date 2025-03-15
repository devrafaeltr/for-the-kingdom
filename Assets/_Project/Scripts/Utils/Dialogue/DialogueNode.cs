using UnityEngine;

namespace FTKingdom
{
    [CreateAssetMenu(fileName = "New DialogueNode", menuName = "Dialogue/Node")]
    public class DialogueNode : ScriptableObject
    {
        // TODO: Maybe create an NpcSO with type, graphic and everything else needed
        [SerializeField] private Sprite speakerSprite;
        // [SerializeField] private NpcType speakerType;
        [TextArea(3, 5)][SerializeField] private string dialogueText;
        [Space]
        [SerializeField] private bool isLeftSpeaker;

        public Sprite SpeakerSprite => speakerSprite;
        // public NpcType SpeakerType => speakerType;
        public string DialogueText => dialogueText;
        public bool IsLeftSpeaker => isLeftSpeaker;
    }
}