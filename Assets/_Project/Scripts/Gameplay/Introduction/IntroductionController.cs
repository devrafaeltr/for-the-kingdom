using UnityEngine;

namespace FTKingdom
{
    public class IntroductionController : MonoBehaviour
    {
        [SerializeField] private DialogueStoryline dialogueStoryline;

        private void Start()
        {
            DialogueController.Instance.StartDialogue(dialogueStoryline);
        }
    }
}