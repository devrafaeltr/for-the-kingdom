using UnityEngine;

namespace FTKingdom
{
    public class MiniMapCameraController : MonoBehaviour
    {
        public float moveSpeed = 0.1f;  // Velocidade do movimento
        public float zoomSpeed = 5f;    // Velocidade do zoom
        public float minZoom = 5f;      // Zoom mínimo (orthographicSize)
        public float maxZoom = 50f;     // Zoom máximo (orthographicSize)

        private Vector3 lastMousePosition;
        private bool isDragging = false;
        private Camera cam;

        void Start()
        {
            cam = Camera.main;
            if (!cam.orthographic)
            {
                Debug.LogWarning("A câmera deve estar em modo ortográfico para este script funcionar corretamente.");
            }
        }

        void Update()
        {
            // Detecta clique do botão direito do mouse
            if (Input.GetMouseButtonDown(1))
            {
                lastMousePosition = Input.mousePosition;
                isDragging = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                isDragging = false;
            }

            // Movimento da câmera ao arrastar o mouse
            if (isDragging)
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                lastMousePosition = Input.mousePosition;

                // Move a câmera horizontalmente e verticalmente
                Vector3 move = new Vector3(-delta.x * moveSpeed * cam.orthographicSize, -delta.y * moveSpeed * cam.orthographicSize, 0);
                transform.position += move;
            }

            // Zoom da câmera com a roda do mouse
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            }
        }
    }
}