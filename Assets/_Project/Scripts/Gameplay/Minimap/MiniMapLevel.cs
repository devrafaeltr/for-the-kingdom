using UnityEngine;

namespace FTKingdom
{
    public class MiniMapLevel : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D levelCollider = null;
        [SerializeField] private SpriteRenderer spriteRenderer = null;

        private LevelState state = LevelState.Blocked;
        private int levelId;

        private void OnMouseDown()
        {
            GameManager.Instance.SetCurrentLevelPlaying(levelId);
            SceneHandler.Instance.LoadScene(GameScene.BattleSite);
        }

        public void InitLevel(int id, LevelState levelState)
        {
            UnityEngine.Debug.Log($"InitLevel: {levelId}");
            levelId = id;
            state = levelState;
            UpdateLevelBehavior();
        }

        public void UnlockLevel(int id)
        {
            InitLevel(id, LevelState.Revealed);
            UpdateLevelBehavior();
        }

        private void UpdateLevelBehavior()
        {
            switch (state)
            {
                case LevelState.Blocked:
                    break;
                case LevelState.Revealed:
                case LevelState.Completed:
                    levelCollider.enabled = true;
                    spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
                    break;
            }

            if (state == LevelState.Revealed)
            {
                spriteRenderer.color = Color.red;
            }
        }
    }
}