using System.Collections.Generic;
using System.Linq;
using FTKingdom.Utils;
using UnityEngine;

namespace FTKingdom
{
    public class BattleSiteManager : LocalSingleton<BattleSiteManager>
    {
        [Header("Resources")]
        [SerializeField] private int baseExp = 10;
        [SerializeField] private int baseGold = 5;
        [SerializeField] private float expModifier = 0.1f;
        [SerializeField] private float goldModifier = 0.05f;

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

        private void OnEnable()
        {
            EventsManager.AddListener(EventsManager.OnBattleEnd, OnBattleEnd);
        }

        private void OnDisable()
        {
            EventsManager.RemoveListener(EventsManager.OnBattleEnd, OnBattleEnd);
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
                GameManager.Instance.MarkCurrentLevelAsCompleted();
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

        // TODO: Get gold based on hero performance.
        public int GetExperienceReward()
        {
            return GetValueWithModifier(baseExp, expModifier);
        }

        // TODO: Get experience based on hero performance.
        public int GetGoldReward()
        {
            return GetValueWithModifier(baseGold, goldModifier);
        }

        private void SetupEnemies()
        {
            EnemyWave currentWave = waves[GameManager.Instance.GetCurrentLevelAsIndex()];
            foreach (WaveEnemy waveEnemy in currentWave.Enemies)
            {
                CharacterBattle enemy = Instantiate(enemyPrefab);
                enemy.Setup(waveEnemy.Enemy);
                enemy.transform.position = enemySlots[waveEnemy.Position].position;
                enemy.CharacterPosition = waveEnemy.Position;
                AddEnemy(enemy);

                BattleSiteCanvas.Instance.AddEnemyToParty(enemy, waveEnemy.Position);
            }
        }

        private void SetupHeroParty()
        {
            List<HeroData> partyHeroes = GameManager.Instance.GetParty();
            foreach (HeroData heroData in partyHeroes)
            {
                HeroBattle hero = Instantiate(heroPrefab);
                hero.Setup(heroData);
                hero.transform.position = heroSlots[heroData.partySlot].position;
                hero.CharacterPosition = heroData.partySlot;
                AddHero(hero);

                BattleSiteCanvas.Instance.AddHeroToParty(hero, heroData.partySlot);
            }
        }

        private void OnBattleEnd(IGameEvent gameEvent)
        {
            // foreach (HeroBattle hero in heroesInBattle.Cast<HeroBattle>())
            // {
            //     hero.AddExperience(GetValueWithModifier(baseExp, expModifier));
            //     hero.AddGold(GetValueWithModifier(baseGold, goldModifier));
            // }

            BattleSiteCanvas.Instance.ShowPostBattle();
        }

        private int GetValueWithModifier(int baseValue, float modifier)
        {
            return baseValue + Mathf.RoundToInt(baseValue * modifier * GameManager.Instance.GetCurrentLevel());
        }
    }
}