using UnityEngine;

namespace ArchivedAlignedGames

{
    public class PlayerCameraManager : MonoBehaviour
    {
        public float mouseSensitivity = 100f;
        public Transform playerBody;

        private float xRotation = 0f;

        void Start()
        {
            // Lock the cursor to the center of the screen
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            HandleCameraLook();
        }

        private void HandleCameraLook()
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate the player horizontally
            playerBody.Rotate(Vector3.up * mouseX);

            // Rotate the camera vertically (clamp to prevent flipping)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

}