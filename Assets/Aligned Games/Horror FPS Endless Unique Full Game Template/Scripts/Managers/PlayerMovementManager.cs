using UnityEngine;

namespace AlignedGames
{
    // Requires the game object to have a CharacterController component
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementManager : MonoBehaviour
    {
        // Player movement variables
        public float walkSpeed = 5f;        // Walking speed
        public float runSpeed = 10f;        // Running speed
        public float crouchSpeed = 2f;      // Crouch speed
        public float proneSpeed = 1f;       // Speed while proning
        public float slideSpeed = 15f;      // Speed while sliding
        public float jumpForce = 10f;       // Jump force
        public float gravity = -9.81f;      // Gravity effect on player
        public float crouchHeight = 0.5f;   // Height when crouching
        public float standHeight = 2f;      // Height when standing
        public float proneHeight = 0.3f;    // Height while proning
        public float slideHeight = 1f;      // Height during slide
        public float slideDuration = 1f;    // Duration of the slide
        public float cameraSmoothTime = 0.2f; // Smooth time for camera movement

        private float ySpeed;               // Vertical speed (for gravity and jumping)
        public float currentSpeed;          // Current movement speed
        public bool isCrouching;            // Flag for crouching state
        public bool isSliding;             // Flag for sliding state
        public bool isProning;             // Flag for proning state
        public bool isRunning;             // Flag for running state
        public bool isWalking;             // Flag for walking state

        public bool isMoving;             // Flag for moving state

        private CharacterController controller;  // Reference to the CharacterController
        private Vector3 moveDirection;          // Direction for movement
        private float slideTimer;               // Timer for slide duration
        private Vector3 cameraInitialPosition;  // Initial camera position
        private Vector3 targetCameraPosition;   // Target camera position for smooth transition
        private Camera playerCamera;            // Reference to the player camera
        private Vector3 cameraVelocity = Vector3.zero; // Velocity for smooth camera movement

        // Initialization
        void Start()
        {
            controller = GetComponent<CharacterController>();  // Get the CharacterController component
            playerCamera = Camera.main;  // Assumes the main camera is attached to the player
            cameraInitialPosition = playerCamera.transform.localPosition;  // Store initial camera position
            currentSpeed = walkSpeed;    // Set initial speed to walking speed
            targetCameraPosition = cameraInitialPosition;  // Initialize target camera position
        }

        // Update is called once per frame
        void Update()
        {
            if (isSliding)
            {
                HandleSlide();  // If sliding, handle the slide behavior
            }
            else
            {
                HandleMovement();  // Handle movement (walking, running)
                HandleCrouch();     // Handle crouching
                HandleProne();      // Handle proning
            }
            HandleJump();  // Handle jumping

            // Smoothly transition the camera's position based on target position
            playerCamera.transform.localPosition = Vector3.SmoothDamp(
                playerCamera.transform.localPosition,
                targetCameraPosition,
                ref cameraVelocity,
                cameraSmoothTime
            );
        }

        // Handle basic player movement (walking, running, crouching, proning)
        private void HandleMovement()
        {
            float moveDirectionY = moveDirection.y;  // Store current vertical speed (for gravity)

            // Get the input for horizontal and vertical movement (left-right, forward-back)
            moveDirection = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")).normalized;

            // Check for running (holding LeftShift), crouching, or proning states
            if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && !isSliding && !isProning)
            {
                currentSpeed = runSpeed;  // Set speed to running if LeftShift is held
            }
            else if (isProning)
            {
                currentSpeed = proneSpeed;  // Set speed for proning
            }
            else
            {
                currentSpeed = isCrouching ? crouchSpeed : walkSpeed;  // Determine walking or crouching speed
            }

            // Update movement states based on currentSpeed
            if (currentSpeed == runSpeed)
            {
                isRunning = true;
                isWalking = false;
            }
            else if (currentSpeed == walkSpeed)
            {
                isRunning = false;
                isWalking = true;
            }
            else
            {
                isRunning = false;
                isWalking = false;
            }

            // Only set isMoving to true if there's movement input
            isMoving = moveDirection.magnitude > 0.1f; // Check if the player is giving input (significant movement)

            // Update isMoving when the player is moving
            if (isMoving)
            {
                isWalking = currentSpeed == walkSpeed;
                isRunning = currentSpeed == runSpeed;
            }
            else
            {
                isWalking = false;
                isRunning = false;
            }

            // Apply movement speed and maintain the vertical speed
            moveDirection *= currentSpeed;
            moveDirection.y = moveDirectionY;

            // Apply gravity to the vertical speed
            ySpeed += gravity * Time.deltaTime;
            moveDirection.y = ySpeed;

            // Move the player based on the calculated direction and speed
            controller.Move(moveDirection * Time.deltaTime);
        }

        // Handle crouching logic (toggle crouch and transition between states)
        private void HandleCrouch()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.C))
            {
                if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && !isSliding && !isProning)
                {
                    StartSlide();  // Start sliding if shift is held and not crouching or proning
                }
                else
                {
                    if (isProning)
                    {
                        // Transition from proning to crouching
                        isProning = false;
                        isCrouching = true;
                        currentSpeed = crouchSpeed;
                        controller.height = crouchHeight;  // Set the height to crouch height
                        targetCameraPosition = new Vector3(
                            playerCamera.transform.localPosition.x,
                            cameraInitialPosition.y - crouchHeight,
                            playerCamera.transform.localPosition.z
                        );
                    }
                    else
                    {
                        // Toggle crouch
                        isCrouching = !isCrouching;
                        if (isCrouching)
                        {
                            currentSpeed = crouchSpeed;
                            controller.height = crouchHeight;  // Set height for crouch
                            targetCameraPosition = new Vector3(
                                playerCamera.transform.localPosition.x,
                                cameraInitialPosition.y - crouchHeight,
                                playerCamera.transform.localPosition.z
                            );
                        }
                        else
                        {
                            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;  // Set speed based on input
                            controller.height = standHeight;  // Reset height to standing
                            targetCameraPosition = new Vector3(
                                playerCamera.transform.localPosition.x,
                                cameraInitialPosition.y,
                                playerCamera.transform.localPosition.z
                            );
                        }
                    }
                }
            }
        }

        // Handle proning logic (toggle proning and transition between states)
        private void HandleProne()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (isCrouching)
                {
                    // Transition from crouching to proning
                    isCrouching = false;
                    isProning = true;
                    currentSpeed = proneSpeed;
                    controller.height = proneHeight;  // Set height to prone height
                    targetCameraPosition = new Vector3(
                        playerCamera.transform.localPosition.x,
                        cameraInitialPosition.y - proneHeight,
                        playerCamera.transform.localPosition.z
                    );
                }
                else
                {
                    // Toggle proning
                    isProning = !isProning;
                    if (isProning)
                    {
                        currentSpeed = proneSpeed;
                        controller.height = proneHeight;  // Set height to prone height
                        targetCameraPosition = new Vector3(
                            playerCamera.transform.localPosition.x,
                            cameraInitialPosition.y - proneHeight,
                            playerCamera.transform.localPosition.z
                        );
                    }
                    else
                    {
                        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;  // Set speed based on input
                        controller.height = standHeight;  // Reset height to standing
                        targetCameraPosition = new Vector3(
                            playerCamera.transform.localPosition.x,
                            cameraInitialPosition.y,
                            playerCamera.transform.localPosition.z
                        );
                    }
                }
            }
        }

        // Handle jump logic (only when grounded and not sliding or proning)
        private void HandleJump()
        {
            if (Input.GetButtonDown("Jump") && controller.isGrounded && !isSliding && !isProning && !isCrouching)
            {
                ySpeed = jumpForce;  // Apply jump force
            }
        }

        // Start the sliding action (set the slide state and update camera)
        private void StartSlide()
        {
            isSliding = true;
            currentSpeed = slideSpeed;
            slideTimer = slideDuration;  // Set slide timer
            isCrouching = true;  // Ensure crouching is active during slide
            isProning = false;   // Ensure proning is disabled during slide
            controller.height = crouchHeight;  // Set height to crouch during slide
            targetCameraPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                cameraInitialPosition.y - slideHeight,
                playerCamera.transform.localPosition.z
            );  // Set target camera position for the slide
        }

        // Handle slide movement and timer
        private void HandleSlide()
        {
            controller.Move(transform.forward * slideSpeed * Time.deltaTime);  // Move forward while sliding
            slideTimer -= Time.deltaTime;  // Reduce slide timer
            if (slideTimer <= 0)
            {
                EndSlide();  // End slide when timer reaches zero
            }
        }

        // End the slide and reset player states and position
        private void EndSlide()
        {
            isSliding = false;
            isCrouching = false;
            isProning = false;
            currentSpeed = walkSpeed;  // Reset speed after sliding
            controller.height = standHeight;  // Reset height to standing
            targetCameraPosition = cameraInitialPosition;  // Reset camera position to initial
        }
    }

}