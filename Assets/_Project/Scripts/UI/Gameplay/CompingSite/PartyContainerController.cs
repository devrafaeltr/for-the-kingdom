using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom.UI
{
    public class PartyContainerController : MonoBehaviour
    {
        [SerializeField] private Transform heroesListContainer = null;
        [SerializeField] private Transform partyContainer = null;

        private List<CharacterUIContainer> currentParty = new();

        private void OnEnable()
        {
            EventsManager.AddListener(EventsManager.OnChangePartyMember, OnChangePartyMember);
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnChangePartyMember, OnChangePartyMember);
        }

        public void CloseParty()
        {
            gameObject.SetActive(false);
        }

        public void SaveParty(bool save)
        {
            if (save)
            {
                // CampingSiteManager.Instance.SetParty(currentParty);
            }
        }

        private void OnChangePartyMember(IGameEvent gameEvent)
        {
            var changePartyEvent = (UpdatePartyEvent)gameEvent;
            OnChangePartyMember(changePartyEvent.OldMember, changePartyEvent.NewMember);
        }

        private void OnChangePartyMember(CharacterUIContainer oldMember, CharacterUIContainer newMember)
        {
            newMember.FitToParent();
            currentParty.Add(newMember);

            if (oldMember == null)
            {
                return;
            }

            oldMember.transform.SetParent(heroesListContainer);
            oldMember.FitToParent();

            currentParty.Remove(oldMember);
        }
    }
}