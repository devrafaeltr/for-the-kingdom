using UnityEngine;
using FTKingdom.Utils;
using TMPro;
using UnityEngine.UI;

namespace FTKingdom
{
    public class DialogueController : LocalSingleton<DialogueController>
    {
        [SerializeField] private GameObject dialoguePanel;

        [Header("Speakers")]
        [SerializeField] private Image leftSpeaker;
        [SerializeField] private Image rightSpeaker;

        [Header("Text infos")]
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI dialogueText;

        private DialogueStoryline currentStoryline;
        private DialogueNode currentNode;
        private int currentNodeIndex = -1;

        private Color transparentColor = new(1, 1, 1, 0);

        public void StartDialogue(DialogueStoryline storyline)
        {
            currentStoryline = storyline;

            if (StorylineEnded())
            {
                return;
            }

            StartDialogue();
        }

        private void StartDialogue()
        {
            dialoguePanel.SetActive(true);
            DisplayNextNode();
        }

        public void DisplayNextNode()
        {
            currentNodeIndex++;

            if (StorylineEnded())
            {
                EndDialogue();
                return;
            }

            currentNode = currentStoryline.DialogueNodes[currentNodeIndex];

            if (currentNode.IsLeftSpeaker)
            {
                leftSpeaker.sprite = currentNode.SpeakerSprite;
                leftSpeaker.color = Color.white;
                rightSpeaker.color = transparentColor;
            }
            else
            {
                rightSpeaker.sprite = currentNode.SpeakerSprite;
                rightSpeaker.color = Color.white;
                leftSpeaker.color = transparentColor;
            }

            speakerName.text = "AuxName";
            dialogueText.text = currentNode.DialogueText;
        }

        private bool StorylineEnded()
        {
            return currentStoryline.DialogueNodes.Count <= currentNodeIndex;
        }

        private void EndDialogue()
        {
            dialoguePanel.SetActive(false);
            currentStoryline = null;
            currentNode = null;
            currentNodeIndex = -1;
        }
    }
}