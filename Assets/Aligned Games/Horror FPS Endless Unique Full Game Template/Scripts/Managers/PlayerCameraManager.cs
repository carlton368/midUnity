using UnityEngine;

namespace AlignedGames
{
    public class PlayerCameraManager : MonoBehaviour
    {
        // Sensitivity for mouse movement along the X-axis (horizontal)
        public float lookSpeedX = 2f;
        // Sensitivity for mouse movement along the Y-axis (vertical)
        public float lookSpeedY = 2f;
        // Upper limit for how far the player can look upwards
        public float upperLookLimit = 80f;
        // Lower limit for how far the player can look downwards
        public float lowerLookLimit = -80f;

        // Variable to store the current vertical camera rotation
        private float rotationX = 0f;

        // Update is called once per frame
        void Update()
        {
            // Get the mouse input for horizontal and vertical movement
            float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

            // Update the vertical rotation based on mouse Y input
            rotationX -= mouseY;
            // Clamp the vertical rotation to prevent going beyond the set limits
            rotationX = Mathf.Clamp(rotationX, lowerLookLimit, upperLookLimit);

            // Apply the vertical rotation to the camera's local rotation
            transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

            // Apply the horizontal rotation to the camera's parent (the player body)
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }
}
