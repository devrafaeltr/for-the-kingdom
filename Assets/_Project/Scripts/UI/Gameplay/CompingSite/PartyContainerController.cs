using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom.UI
{
    public class PartyContainerController : MonoBehaviour
    {
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
            // var originaPoisiion = newMember original position
            // newMember position = oldMember position
            // oldMember position = originaPoisiion

            // if (oldMember == null)
            // {
            //     return;
            // }

            // (newMember.transform.position, oldMember.transform.position) = (oldMember.transform.position, newMember.transform.position);
        }
    }
}