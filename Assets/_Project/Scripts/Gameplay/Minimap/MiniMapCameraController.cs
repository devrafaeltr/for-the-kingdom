using UnityEngine;

namespace FTKingdom
{
    public class MiniMapCameraController : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float minX = -50f;
        [SerializeField] private float maxX = 50f;
        [SerializeField] private float minY = -50f;
        [SerializeField] private float maxY = 50f;

        [Header("Zoom")]
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 70f;

        private Vector3 lastMousePosition;
        private bool isDragging = false;

        void Update()
        {
            // TODO: Maybe need to change if/when using new input system
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePosition = Input.mousePosition;
                isDragging = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Vector3 delta = cam.orthographicSize * moveSpeed * 0.01f * (lastMousePosition - Input.mousePosition);
                Vector3 newPosition = transform.position + delta;

                // Aplicar limites
                newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

                transform.position = newPosition;
                lastMousePosition = Input.mousePosition;
            }

            // TODO: Maybe need to change if/when using new input system
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            }
        }
    }
}