using System.Collections.Generic;
using System.Linq;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom.UI
{
    public class PartyContainerController : LocalSingleton<PartyContainerController>
    {
        [SerializeField] private Transform heroesListContainer = null;
        [SerializeField] private CharacterUIContainer characterUIPrefab = null;
        [SerializeField] private List<UIPartySlot> partySlots = new();

        private float canvasScale;

        private void OnEnable()
        {
            canvasScale = GetComponentInParent<Canvas>().scaleFactor;
            SetupHeroes();
            EventsManager.AddListener(EventsManager.OnChangePartyMember, OnChangePartyMember);
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnChangePartyMember, OnChangePartyMember);
            foreach (var heroObj in auxHeroes)
            {
                Destroy(heroObj);
            }

            auxHeroes.Clear();
        }

        public void AddToParty(CharacterUIContainer newMember, UIPartySlot slot)
        {
            newMember.FitToParent();
            newMember.ChangePartySlot(partySlots.IndexOf(slot));
        }

        public void RemoveFromParty(CharacterUIContainer oldMember)
        {
            oldMember.transform.SetParent(heroesListContainer);
            oldMember.FitToParent();
            oldMember.ChangePartySlot(-1);
        }

        public void CloseParty()
        {
            gameObject.SetActive(false);
        }

        // TODO: Generic pool
        private List<GameObject> auxHeroes = new();
        private void SetupHeroes()
        {
            foreach (var hero in GameManager.Instance.CurrentHeroes)
            {
                CharacterUIContainer heroUIObj = Instantiate(characterUIPrefab);
                heroUIObj.Setup(hero, canvasScale);

                if (hero.IsOnParty)
                {
                    heroUIObj.transform.SetParent(partySlots[hero.PartySlot].transform);
                    partySlots[hero.PartySlot].SetCurrentMember(heroUIObj);
                }
                else
                {
                    heroUIObj.transform.SetParent(heroesListContainer);
                }

                heroUIObj.FitToParent();
                auxHeroes.Add(heroUIObj.gameObject);
            }
        }

        private void OnChangePartyMember(IGameEvent gameEvent)
        {
            var changePartyEvent = (UpdatePartyEvent)gameEvent;
            OnChangePartyMember(changePartyEvent.Slot, changePartyEvent.OldMember, changePartyEvent.NewMember);
        }

        private void OnChangePartyMember(UIPartySlot slot, CharacterUIContainer oldMember, CharacterUIContainer newMember)
        {
            AddToParty(newMember, slot);

            if (oldMember == null)
            {
                return;
            }

            RemoveFromParty(oldMember);
        }
    }
}