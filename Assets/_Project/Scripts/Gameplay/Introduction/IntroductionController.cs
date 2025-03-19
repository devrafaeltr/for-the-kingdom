using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;
using UnityEngine.Playables;

namespace FTKingdom
{
    public class IntroductionController : LocalSingleton<IntroductionController>
    {
        [SerializeField] private PlayableDirector timelineDirector;
        [SerializeField] private List<DialogueStoryline> dialogueStorylines;

        private int storylineIndex = -1;

        private void OnEnable()
        {
            EventsManager.AddListener(EventsManager.OnCurrentStorylineEnds, OnCurrentStorylineEnds);
            timelineDirector.Play();
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnCurrentStorylineEnds, OnCurrentStorylineEnds);
        }

        public void OnEnterDilogueTimestamp()
        {
            timelineDirector.Pause();
            PlayNextDialogue();
        }

        private void PlayNextDialogue()
        {
            storylineIndex++;
            if (storylineIndex < dialogueStorylines.Count)
            {
                DialogueController.Instance.StartDialogue(dialogueStorylines[storylineIndex]);
            }
        }

        private void OnCurrentStorylineEnds(IGameEvent gameEvent)
        {
            timelineDirector.Resume();
        }
    }
}