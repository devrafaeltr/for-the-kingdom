using UnityEngine;

namespace FTKingdom
{
    [System.Serializable]
    public class DialogueLine
    {
        [SerializeField, HideInInspector] private string dialogueKey;

        // TODO: Maybe create an NpcSO with type, graphic and everything else needed
        [SerializeField] private Sprite speakerSprite;
        [SerializeField] private bool isLeftSpeaker;
        [SerializeField, ReadOnly] private string dialogueText = "Dialogue preview";

        public string DialogueKey => dialogueKey;
        public Sprite SpeakerSprite => speakerSprite;
        public bool IsLeftSpeaker => isLeftSpeaker;
        public string DialogueText => dialogueText;
    }
}