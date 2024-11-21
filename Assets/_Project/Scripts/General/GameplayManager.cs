using System.Collections.Generic;
using System.Linq;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class GameplayManager : LocalSingleton<GameplayManager>
    {
        private List<CharacterBattle> heroesInBattle = new();
        private List<CharacterBattle> enemiesInBattle = new();

        private void Awake()
        {

            heroesInBattle = FindObjectsOfType<CharacterBattle>().ToList();

            var enemy = heroesInBattle.Find(h => h.characerData.Type == CharacterType.Enemy);
            heroesInBattle.Remove(enemy);
            enemiesInBattle.Add(enemy);
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
    }
}