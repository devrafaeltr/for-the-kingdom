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

        private float canvasScale;

        private void Awake()
        {
            canvasScale = GetComponentInParent<Canvas>().scaleFactor;
            GenericPool.CreatePool(PoolType.CharacerUI, characterUIPrefab);
        }

        private void OnEnable()
        {
            SetupHeroes();
            EventsManager.AddListener(EventsManager.OnChangePartyMember, OnChangePartyMember);
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnChangePartyMember, OnChangePartyMember);
        }

        public void AddToParty(CharacterUIContainer newMember, UIPartySlot slot)
        {
            newMember.ChangePartySlot(partySlots.IndexOf(slot), slot);
        }

        public void RemoveFromParty(CharacterUIContainer member)
        {
            member.transform.SetParent(heroesListContainer);
            member.RemoveFromParty();
        }

        public void CloseParty()
        {
            gameObject.SetActive(false);
        }

        private void SetupHeroes()
        {
            foreach (var hero in GameManager.Instance.CurrentHeroes)
            {
                GenericPool.GetItem(out CharacterUIContainer heroUIObj, PoolType.CharacerUI);
                heroUIObj.Setup(hero, canvasScale);

                if (hero.IsOnParty)
                {
                    AddToParty(heroUIObj, partySlots[hero.PartySlot]);
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
                    Debug.Log($"Swap party members.");
                    AddToParty(currentMember, newMember.CurrentSlot);
                    AddToParty(newMember, slot);
                }
                else
                {
                    Debug.Log($"Substitute party members.");
                    RemoveFromParty(currentMember);
                    AddToParty(newMember, slot);
                }
            }
            else
            {
                if (newMember.IsOnParty)
                {
                    Debug.Log($"Change party slot.");
                    newMember.CurrentSlot.RemoveMember();
                    AddToParty(newMember, slot);
                }
                else
                {
                    Debug.Log($"Assign party member.");
                    AddToParty(newMember, slot);
                }
            }

            // AddToParty(newMember, slot);
        }
    }
}