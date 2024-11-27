using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public List<CharacterSO> auxHeroesInfos = new();

        private readonly List<Character> currentHeroes = new();
        public List<Character> CurrentHeroes => currentHeroes;

        protected override void Awake()
        {
            base.Awake();
            SceneHandler.Instance.LoadScene(GameScene.MainMenu);

            for (int i = 0; i < 15; i++)
            {
                Character c = new();
                c.SetCharacterData(auxHeroesInfos[Random.Range(0, auxHeroesInfos.Count)]);
                currentHeroes.Add(c);
            }

            for (int i = 0; i < 5; i++)
            {
                currentHeroes[Random.Range(0, currentHeroes.Count)].SetPartySlot(i);
            }
        }

        public List<Character> GetParty()
        {
            return currentHeroes.FindAll(c => c.IsOnParty);
        }
    }
}