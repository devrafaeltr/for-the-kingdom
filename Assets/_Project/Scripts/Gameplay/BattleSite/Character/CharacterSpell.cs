using UnityEngine;

namespace FTKingdom
{
    [System.Serializable]
    public class CharacterSpell
    {
        public BaseSpellSO spellData;
        public float currentCooldown = 0;

        public bool CanUse => currentCooldown <= 0;

        public CharacterSpell(BaseSpellSO spell)
        {
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
            // Whatever
            Debug.Log($"Using {spellData.Name}!");
            var p = Object.Instantiate(spellData.ProjectileData.ProjectilePrefab, spawnPos, Quaternion.identity)
            .GetComponent<Projectile>();
            p.Setup(10, spellData.ProjectileData, target);
            ResetCooldown();
        }
    }
}