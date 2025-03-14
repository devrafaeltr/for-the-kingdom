using UnityEngine;
using TMPro;
using DG.Tweening;

namespace FTKingdom
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1.0f;
        [SerializeField] private float fadeDuration = 1.0f;
        [SerializeField] private float moveHeight = 1.0f;
        [SerializeField] private TextMeshProUGUI txtFloatingText;

        public void Setup(string text, Vector2 position, Color color)
        {
            transform.position = position;
            txtFloatingText.color = color;
            SetText(text);
        }

        private void SetText(string message)
        {
            if (txtFloatingText == null)
            {
                return;
            }

            txtFloatingText.text = message;
            txtFloatingText.DOFade(1, 0);

            DOTween.Sequence()
            .Append(transform.DOMoveY(transform.position.y + moveHeight, moveHeight / moveSpeed).SetEase(Ease.OutQuad))
            .Join(txtFloatingText.DOFade(0, fadeDuration))
            .OnComplete(() => Destroy(gameObject))
            .Play();
        }
    }
}