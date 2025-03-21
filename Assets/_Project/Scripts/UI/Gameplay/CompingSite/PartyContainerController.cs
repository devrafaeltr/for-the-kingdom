using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom.UI
{
    public class PartyContainerController : LocalSingleton<PartyContainerController>
    {
        [SerializeField] private Transform heroesListContainer = null;
        [SerializeField] private CharacterUIContainer characterUIPrefab = null;
        [SerializeField] private List<UIPartySlot> partySlots = new();

        private readonly Dictionary<CharacterUIContainer, UIPartySlot> membersToSlot = new();

        private float canvasScale;

        private void Awake()
        {
            canvasScale = GetComponentInParent<Canvas>().scaleFactor;
            GenericPool.CreatePool(PoolType.CharacerUI, characterUIPrefab);
        }

        private void OnEnable()
        {
            AuxSetupHeroes();
            EventsManager.AddListener(EventsManager.OnChangePartyMember, OnChangePartyMember);
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnChangePartyMember, OnChangePartyMember);
        }

        public void RemoveFromParty(CharacterUIContainer member)
        {
            if (membersToSlot.ContainsKey(member))
            {
                membersToSlot[member].RemoveMember();
                membersToSlot.Remove(member);
            }

            member.transform.SetParent(heroesListContainer);
            member.RemoveFromParty();
        }

        public void CloseParty()
        {
            gameObject.SetActive(false);
        }

        private void AddToParty(CharacterUIContainer newMember, UIPartySlot slot)
        {
            if (!membersToSlot.TryGetValue(newMember, out UIPartySlot currentSlot))
            {
                membersToSlot.Add(newMember, slot);
            }
            else
            {
                currentSlot.RemoveMember();
                membersToSlot[newMember] = slot;
            }

            newMember.transform.SetParent(slot.transform);
            newMember.ChangePartySlot(partySlots.IndexOf(slot));
            slot.SetCurrentMember(newMember);
        }

        // TODO: Find out why its Aux.
        private void AuxSetupHeroes()
        {
            foreach (var hero in GameManager.Instance.CurrentHeroes)
            {
                GenericPool.GetItem(out CharacterUIContainer heroUIObj, PoolType.CharacerUI);
                heroUIObj.Setup(hero, canvasScale);

                if (hero.IsOnParty)
                {
                    AddToParty(heroUIObj, partySlots[hero.partySlot]);
                }
                else
                {
                    RemoveFromParty(heroUIObj);
                }
            }
        }

        private void OnChangePartyMember(IGameEvent gameEvent)
        {
            var changePartyEvent = (UpdatePartyEvent)gameEvent;
            OnChangePartyMember(changePartyEvent.Slot, changePartyEvent.CurrentMember, changePartyEvent.NewMember);
        }

        private void OnChangePartyMember(UIPartySlot slot, CharacterUIContainer currentMember, CharacterUIContainer newMember)
        {
            if (currentMember != null)
            {
                if (newMember.IsOnParty)
                {
                    AddToParty(currentMember, membersToSlot[newMember]);
                    membersToSlot.Remove(newMember);
                }
                else
                {
                    RemoveFromParty(currentMember);
                }
            }

            AddToParty(newMember, slot);
        }
    }
}