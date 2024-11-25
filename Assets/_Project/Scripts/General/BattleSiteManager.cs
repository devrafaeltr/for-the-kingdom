using System.Collections.Generic;
using System.Linq;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class BattleSiteManager : LocalSingleton<BattleSiteManager>
    {
        [SerializeField] private CharacterBattle heroPrefab = null;
        [SerializeField] private List<Transform> heroSlots = new();
        private readonly List<CharacterBattle> heroesInBattle = new();
        private List<CharacterBattle> enemiesInBattle = new();

        private void Awake()
        {
            enemiesInBattle = GameObject.FindGameObjectsWithTag("Enemy").Select(e => e.GetComponent<CharacterBattle>()).ToList();
            SetupHeroParty();
        }

        public CharacterBattle GetClosestFromType(Vector2 position, CharacterType type)
        {
            if (type == CharacterType.Enemy)
            {
                return SortCharacterList(enemiesInBattle, position);
            }

            return SortCharacterList(heroesInBattle, position);

            CharacterBattle SortCharacterList(List<CharacterBattle> characters, Vector2 position)
            {
                characters.Sort((e1, e2) =>
                {
                    var dist1 = Vector3.Distance(e1.transform.position, position);
                    var dist2 = Vector3.Distance(e2.transform.position, position);

                    return dist1.CompareTo(dist2);
                });

                return enemiesInBattle[0];
            }
        }

        private void SetupHeroParty()
        {
            List<Character> partyHeroes = GameManager.Instance.GetParty();
            foreach (Character hero in partyHeroes)
            {
                CharacterBattle heroObj = Instantiate(heroPrefab);
                heroObj.Setup(hero);
                heroObj.transform.position = heroSlots[hero.PartySlot].position;

                heroesInBattle.Add(heroObj);
            }
        }
    }
}