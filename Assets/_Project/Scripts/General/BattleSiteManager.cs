using System.Collections.Generic;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class BattleSiteManager : LocalSingleton<BattleSiteManager>
    {
        [Header("Heroes")]
        [SerializeField] private HeroBattle heroPrefab = null;
        [SerializeField] private CharacterBattle enemyPrefab = null;

        [Header("Enemies")]
        [SerializeField] private List<Transform> heroSlots = new();
        [SerializeField] private List<Transform> enemySlots = new();

        // TODO: Maybe remove from here..... Monitoring.
        [SerializeField] private List<EnemyWave> waves = new();

        private List<Transform> heroesInBattle = new();
        private List<Transform> enemiesInBattle = new();

        private void Awake()
        {
            SetupHeroParty();
            SetupEnemies();
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
                if (characters.Count == 0)
                {
                    return null;
                }
                else if (characters.Count == 1)
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

        public void RemoveHero(Transform hero)
        {
            heroesInBattle.Remove(hero);
        }

        public void RemoveEnemy(Transform enemy)
        {
            enemiesInBattle.Remove(enemy);

            if (enemiesInBattle.Count == 0)
            {
                // TODO: Add a timer after the last enemy die, then call it completed, if aliv hero.
                GameManager.Instance.UpdateLastCompletedLevel();
            }
        }

        public void AddHero(Transform hero)
        {
            heroesInBattle.Add(hero);
        }

        public void AddEnemy(Transform enemy)
        {
            enemiesInBattle.Add(enemy);
        }

        private void SetupEnemies()
        {
            EnemyWave currentWave = waves[GameManager.Instance.GetCurrentLevelPlaying()];
            foreach (WaveEnemy waveEnemy in currentWave.Enemies)
            {
                CharacterBattle enemyObj = Instantiate(enemyPrefab);
                enemyObj.Setup(waveEnemy.Enemy);
                enemyObj.transform.position = enemySlots[waveEnemy.Position].position;
                AddEnemy(enemyObj.transform);
            }
        }

        private void SetupHeroParty()
        {
            List<HeroData> partyHeroes = GameManager.Instance.GetParty();
            foreach (HeroData hero in partyHeroes)
            {
                HeroBattle heroObj = Instantiate(heroPrefab);
                heroObj.Setup(hero);
                heroObj.transform.position = heroSlots[hero.PartySlot].position;
                AddHero(heroObj.transform);
            }
        }
    }
}