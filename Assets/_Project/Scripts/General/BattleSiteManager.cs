using System.Collections.Generic;
using System.Linq;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class BattleSiteManager : LocalSingleton<BattleSiteManager>
    {
        [SerializeField] private HeroBattle heroPrefab = null;
        [SerializeField] private List<Transform> heroSlots = new();
        private List<Transform> heroesInBattle = new();
        private List<Transform> enemiesInBattle = new();

        private void Awake()
        {
            enemiesInBattle = GameObject.FindGameObjectsWithTag("Enemy").Select(e => e.transform).ToList();
            SetupHeroParty();
        }

        public Transform GetClosestFromType(Vector2 position, CharacterType type)
        {
            if (type == CharacterType.Enemy)
            {
                return SortCharacterList(enemiesInBattle, position);
            }

            return SortCharacterList(heroesInBattle, position);

            Transform SortCharacterList(List<Transform> characters, Vector2 position)
            {
                if (characters.Count == 1)
                {
                    return characters[0];
                }
                else if (characters.Count == 2)
                {
                    return Vector3.Distance(characters[0].position, position) <
                    Vector3.Distance(characters[1].position, position) ? characters[0] : characters[1];
                }

                characters.Sort((e1, e2) =>
                {
                    var dist1 = Vector3.Distance(e1.transform.position, position);
                    var dist2 = Vector3.Distance(e2.transform.position, position);

                    return dist1.CompareTo(dist2);
                });

                return characters[0];
            }
        }

        private void SetupHeroParty()
        {
            List<Character> partyHeroes = GameManager.Instance.GetParty();
            foreach (Character hero in partyHeroes)
            {
                HeroBattle heroObj = Instantiate(heroPrefab);
                heroObj.Setup(hero);
                heroObj.transform.position = heroSlots[hero.PartySlot].position;

                heroesInBattle.Add(heroObj.transform);
            }
        }
    }
}