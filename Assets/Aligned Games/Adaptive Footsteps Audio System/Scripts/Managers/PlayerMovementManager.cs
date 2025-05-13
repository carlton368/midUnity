using UnityEngine;

namespace ArchivedAlignedGames

{
    public class PlayerMovementManager : MonoBehaviour
    {
        public float walkSpeed = 5f;             // Normal walking speed
        public float runSpeed = 10f;             // Speed while running
        public float jumpHeight = 2f;            // Jump height
        public float gravity = -9.81f;           // Gravity value
        public float groundCheckDistance = 0.1f; // Distance for ground check
        public LayerMask[] groundLayer;          // Layer(s) considered as ground (Array)

        private CharacterController characterController;
        private Vector3 velocity;

        public bool isGrounded { get; private set; } // Exposed for external use
        public bool isMoving { get; private set; }   // Exposed for external use
        public bool isRunning { get; private set; }  // Exposed for external use

        void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            // Combine all ground layers into one LayerMask using bitwise OR
            int combinedGroundLayer = 0;
            foreach (LayerMask layer in groundLayer)
            {
                combinedGroundLayer |= layer.value; // Combine the layers
            }

            // Check if the player is grounded using a raycast with the combined ground layer mask
            isGrounded = Physics.Raycast(transform.position, Vector3.down,
                groundCheckDistance + characterController.skinWidth, combinedGroundLayer);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = 0f; // Reset vertical velocity when grounded
            }

            // Get movement input
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;

            // Determine whether the player is running
            isRunning = Input.GetKey(KeyCode.LeftShift);

            // Use run speed if running, otherwise use walk speed
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            // Move the player
            characterController.Move(move * currentSpeed * Time.deltaTime);

            // Update isMoving flag
            isMoving = move.magnitude > 0.1f && isGrounded;

            // Jump input
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            // Visualize the ground check ray in the Editor
            Gizmos.color = Color.red;
            if (characterController != null)
            {
                Vector3 rayOrigin = transform.position;
                Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * (groundCheckDistance + characterController.skinWidth));
            }
        }
    }

}