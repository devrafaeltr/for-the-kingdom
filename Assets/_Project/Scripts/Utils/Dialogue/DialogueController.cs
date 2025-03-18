using UnityEngine;
using FTKingdom.Utils;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

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
        private DialogueLine currentLine;
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

            currentLine = currentStoryline.DialogueLines[currentNodeIndex];

            if (currentLine.IsLeftSpeaker)
            {
                leftSpeaker.sprite = currentLine.SpeakerSprite;
                leftSpeaker.color = Color.white;
                rightSpeaker.color = transparentColor;
            }
            else
            {
                rightSpeaker.sprite = currentLine.SpeakerSprite;
                rightSpeaker.color = Color.white;
                leftSpeaker.color = transparentColor;
            }

            // TODO: Add speaker name
            speakerName.text = "MissingName";
            dialogueText.text = LocalizationHelper.GetLocalizedText(currentLine.DialogueKey);
        }

        private bool StorylineEnded()
        {
            return currentStoryline.DialogueLines.Count <= currentNodeIndex;
        }

        private void EndDialogue()
        {
            dialoguePanel.SetActive(false);
            currentStoryline = null;
            currentLine = null;
            currentNodeIndex = -1;
        }
    }
}