using UnityEngine;

namespace FTKingdom
{
    [System.Serializable]
    public class CharacterSpell
    {
        public BaseSpellSO spellData;
        public float currentCooldown = 0;

        public bool CanUse => currentCooldown <= 0;

        private readonly CharacterType casterType;

        public CharacterSpell(BaseSpellSO spell, CharacterType type)
        {
            casterType = type;
            spellData = spell;
            ResetCooldown();
        }

        public void ResetCooldown()
        {
            currentCooldown = spellData.Cooldown;
        }

        public void DoCooldownProgress()
        {
            if (currentCooldown <= 0)
            {
                return;
            }

            currentCooldown -= Time.deltaTime;

            if (currentCooldown < 0)
            {
                currentCooldown = 0;
            }
        }

        public void Use(Vector3 spawnPos, Transform target)
        {
            // TODO: Move to ProjectileManager or something to be able to pool projectiles.
            var p = Object.Instantiate(spellData.ProjectileData.ProjectilePrefab, spawnPos, Quaternion.identity)
            .GetComponent<Projectile>();
            // TODO: Pass spell infos instead of 10
            // Maybe damange will be calculated instead of define on spell.
            p.Setup(10, casterType, spellData.ProjectileData, target, spellData.BehaviorType);
            ResetCooldown();
        }
    }
}