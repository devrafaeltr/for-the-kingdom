using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    private int maxMana = 0;
    private int maxHp = 0;
    private int currentMana = 0;
    private int currentHp = 0;

    private void Awake()
    {

    }

    private void Update()
    {

    }

    protected virtual void Damage(int damage)
    {
        currentHp -= damage;

        if (currentHp > 0)
        {
            UpdatePointsBar(maxHp, currentHp);
            return;
        }

        Die();
    }

    private void UpdatePointsBar(int max, int current)
    {
        float percent = max / current;
    }

    private void Die()
    {
        currentHp = 0;
    }
}
