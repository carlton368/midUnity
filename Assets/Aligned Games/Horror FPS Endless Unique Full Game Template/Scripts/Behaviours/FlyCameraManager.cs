using UnityEngine;
using System.Collections;

namespace AlignedGames
{
    public class FlyCameraManager : MonoBehaviour
    {
        public float cameraSensitivity = 90; // Sensitivity for mouse movement
        public float climbSpeed = 4; // Speed for vertical movement (up/down)
        public float normalMoveSpeed = 10; // Default movement speed
        public float slowMoveFactor = 0.25f; // Speed multiplier when moving slowly
        public float fastMoveFactor = 3; // Speed multiplier when moving fast

        private float rotationX = 0.0f; // Stores X-axis rotation (horizontal)
        private float rotationY = 0.0f; // Stores Y-axis rotation (vertical)

        void Start()
        {
            Screen.lockCursor = true; // Locks the cursor to the screen center
        }

        void Update()
        {
            // Get mouse movement input and apply sensitivity scaling
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90); // Clamp vertical rotation to prevent flipping

            // Apply rotation to the camera
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

            // Movement controls with speed adjustments based on key inputs
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) // Fast movement
            {
                transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) // Slow movement
            {
                transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
            }
            else // Normal movement
            {
                transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            }

            // Vertical movement controls (Q = up, E = down)
            if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
            if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }

            // Toggle cursor lock when pressing the End key
            if (Input.GetKeyDown(KeyCode.End))
            {
                Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
            }
        }
    }
}
