using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBarPointController : MonoBehaviour
{
    [SerializeField] private GameObject barsContainer = null;
    [SerializeField] private Image imgHealthPoints = null;
    [SerializeField] private Image imgManaPoints = null;

    private bool shouldDisable = false;

    public void UpdateHealthPoints(int healthPoints, int maxHealthPoints)
    {
        float targetFillAmount = (float)healthPoints / maxHealthPoints;
        imgHealthPoints.DOFillAmount(targetFillAmount, 0.2f).OnComplete(CheckDisable);
    }

    public void UpdateManaPoints(int manaPoints, int maxManaPoints)
    {
        float targetFillAmount = (float)manaPoints / maxManaPoints;
        imgManaPoints.DOFillAmount(targetFillAmount, 0.2f).OnComplete(CheckDisable);
    }

    public void Disable()
    {
        shouldDisable = true;
    }

    private void CheckDisable()
    {
        if (shouldDisable)
        {
            barsContainer.SetActive(false);
        }
    }
}
