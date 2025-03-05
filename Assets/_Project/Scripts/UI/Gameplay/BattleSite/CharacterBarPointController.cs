using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBarPointController : MonoBehaviour
{
    [SerializeField] private Image imgHealthPoints = null;
    [SerializeField] private Image imgManaPoints = null;

    public void UpdateHealthPoints(int healthPoints, int maxHealthPoints)
    {
        float targetFillAmount = (float)healthPoints / maxHealthPoints;
        imgHealthPoints.DOFillAmount(targetFillAmount, 0.2f);
    }

    public void UpdateManaPoints(int manaPoints, int maxManaPoints)
    {
        float targetFillAmount = (float)manaPoints / maxManaPoints;
        imgManaPoints.DOFillAmount(targetFillAmount, 0.2f);
    }
}
