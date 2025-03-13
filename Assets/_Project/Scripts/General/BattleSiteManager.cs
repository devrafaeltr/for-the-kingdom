using System.Collections.Generic;
using FTKingdom.UI;
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

        private readonly List<CharacterBattle> heroesInBattle = new();
        private readonly List<CharacterBattle> enemiesInBattle = new();
        public List<CharacterBattle> HeroesInBattle
        {
            get => heroesInBattle;
        }

        public List<CharacterBattle> EnemiesInBattle
        {
            get => enemiesInBattle;
        }

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

            Transform SortCharacterList(List<CharacterBattle> characters, Vector2 position)
            {
                if (characters == null || characters.Count == 0)
                {
                    return null;
                }

                Transform closestTransform = null;
                float closestDistance = float.MaxValue;

                foreach (var character in characters)
                {
                    float distance = Vector3.Distance(character.Transform.position, position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTransform = character.Transform;
                    }
                }

                return closestTransform;
            }
        }

        public void RemoveHero(CharacterBattle hero)
        {
            heroesInBattle.Remove(hero);
        }

        public void RemoveEnemy(CharacterBattle enemy)
        {
            enemiesInBattle.Remove(enemy);

            if (enemiesInBattle.Count == 0)
            {
                // TODO: Add a timer after the last enemy die, then call it completed, if aliv hero.
                GameManager.Instance.UpdateLastCompletedLevel();
            }
        }

        public void AddHero(CharacterBattle hero)
        {
            heroesInBattle.Add(hero);
        }

        public void AddEnemy(CharacterBattle enemy)
        {
            enemiesInBattle.Add(enemy);
        }

        private void SetupEnemies()
        {
            EnemyWave currentWave = waves[GameManager.Instance.GetCurrentLevelPlaying()];
            foreach (WaveEnemy waveEnemy in currentWave.Enemies)
            {
                CharacterBattle enemy = Instantiate(enemyPrefab);
                enemy.Setup(waveEnemy.Enemy);
                enemy.transform.position = enemySlots[waveEnemy.Position].position;
                AddEnemy(enemy);

                BattleSiteCanvas.Instance.AddEnemyToParty(enemy, waveEnemy.Position);
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
                AddHero(heroObj);

                BattleSiteCanvas.Instance.AddHeroToParty(heroObj, hero.PartySlot);
            }
        }
    }
}