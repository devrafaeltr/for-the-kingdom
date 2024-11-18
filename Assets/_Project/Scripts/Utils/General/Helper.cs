using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace FTKingdom.Utils
{
    public static class Helper
    {
        private static Camera mainCamera;
        private static Camera MainCamera
        {
            get
            {
                if (mainCamera == null)
                {
                    mainCamera = Camera.main;
                }

                return mainCamera;
            }
        }

        public static Vector2 GetWorldMousePos()
        {
            Vector2 mousePos = Mouse.current.position.value;
            var worldPos = MainCamera.ScreenToWorldPoint(mousePos);
            return worldPos;
        }

        public static GameObject GetObjectInPosition(Vector2 position)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
            return hit.collider != null ? hit.collider.gameObject : null;
        }

        public static GameObject GetObjectInMousePos()
        {
            return GetObjectInPosition(GetWorldMousePos());
        }

        public static GameObject GetObjectInMousePos(LayerMask mask)
        {
            RaycastHit2D hit = Physics2D.Raycast(GetWorldMousePos(), Vector2.zero, 100f, mask);
            return hit.collider != null ? hit.collider.gameObject : null;
        }

        public static bool IsMouseOutsideUI()
        {
            return !EventSystem.current.IsPointerOverGameObject(0);
        }
    }
}