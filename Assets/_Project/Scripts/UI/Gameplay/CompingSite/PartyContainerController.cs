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
        [SerializeField] private List<Transform> partySlots = new();

        private List<CharacterUIContainer> editingParty = new();

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

        public void AddToParty(CharacterUIContainer newMember)
        {
            editingParty.Add(newMember);
            newMember.FitToParent();
        }

        public void RemoveFromParty(CharacterUIContainer oldMember)
        {
            editingParty.Remove(oldMember);
            oldMember.transform.SetParent(heroesListContainer);
            oldMember.FitToParent();
        }

        public void CloseParty()
        {
            gameObject.SetActive(false);
            SaveParty(true);
        }

        public void SaveParty(bool save)
        {
            if (save)
            {
                GameManager.Instance.UpdateParty(editingParty.Select(c => c.character).ToList());
            }
        }

        private List<GameObject> auxHeroes = new();
        private void SetupHeroes()
        {
            foreach (var hero in GameManager.Instance.CurrentHeroes)
            {
                // TODO: Generic pool
                CharacterUIContainer heroUIObj = Instantiate(characterUIPrefab);
                heroUIObj.Setup(hero, canvasScale);

                if (hero.IsOnParty)
                {
                    editingParty.Add(heroUIObj);
                    heroUIObj.transform.SetParent(partySlots[hero.PartySlot]);
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
            OnChangePartyMember(changePartyEvent.OldMember, changePartyEvent.NewMember);
        }

        private void OnChangePartyMember(CharacterUIContainer oldMember, CharacterUIContainer newMember)
        {
            AddToParty(newMember);

            if (oldMember == null)
            {
                return;
            }

            RemoveFromParty(oldMember);
        }
    }
}