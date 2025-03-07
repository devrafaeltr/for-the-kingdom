using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public List<CharacterSO> auxHeroesInfos = new();

        private readonly List<HeroData> currentHeroes = new();
        public List<HeroData> CurrentHeroes => currentHeroes;

        public int LevelPlaying = 0;

        protected override void Awake()
        {
            base.Awake();
            SceneHandler.Instance.LoadScene(GameScene.MainMenu);

            // TODO: Remove after implementing hero arrival
            for (int i = 0; i < 15; i++)
            {
                HeroData c = new();
                c.SetCharacterData(auxHeroesInfos[Random.Range(0, auxHeroesInfos.Count)]);
                currentHeroes.Add(c);
            }

            // TODO: Remove after implementing hero arrival.
            // Add only first hero by default, but probably no here.
            for (int i = 0; i < 5; i++)
            {
                currentHeroes[Random.Range(0, currentHeroes.Count)].SetPartySlot(i);
            }
        }

        public List<HeroData> GetParty()
        {
            return currentHeroes.FindAll(c => c.IsOnParty);
        }
    }
}