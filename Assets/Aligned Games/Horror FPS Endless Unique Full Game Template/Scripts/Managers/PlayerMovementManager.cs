using UnityEngine;

namespace AlignedGames
{
    // Requires the game object to have a CharacterController component
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementManager : MonoBehaviour
    {
        // 사용자 정의 이동 키
        public KeyCode moveForwardKey = KeyCode.W;
        public KeyCode moveBackwardKey = KeyCode.S;
        public KeyCode moveLeftKey = KeyCode.A;
        public KeyCode moveRightKey = KeyCode.D;
        
        // 추가 기능 키
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode runKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.LeftControl;
        public KeyCode proneKey = KeyCode.Z;

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

            // 사용자 정의 키를 사용한 이동 입력 처리
            float horizontalInput = 0f;
            float verticalInput = 0f;

            // 수평 이동 처리
            if (Input.GetKey(moveRightKey)) horizontalInput += 1f;
            if (Input.GetKey(moveLeftKey)) horizontalInput -= 1f;
            
            // 수직 이동 처리
            if (Input.GetKey(moveForwardKey)) verticalInput += 1f;
            if (Input.GetKey(moveBackwardKey)) verticalInput -= 1f;

            // 이동 방향 계산 (정규화 적용)
            Vector3 moveInput = (transform.right * horizontalInput + transform.forward * verticalInput);
            if (moveInput.magnitude > 1f)
                moveInput.Normalize();
                
            moveDirection = moveInput;

            // 달리기, 웅크리기, 엎드리기 상태 확인
            if (Input.GetKey(runKey) && !isCrouching && !isSliding && !isProning)
            {
                currentSpeed = runSpeed;  // 달리기 속도 적용
            }
            else if (isProning)
            {
                currentSpeed = proneSpeed;  // 엎드리기 속도 적용
            }
            else
            {
                currentSpeed = isCrouching ? crouchSpeed : walkSpeed;  // 웅크리기 또는 걷기 속도 결정
            }

            // 현재 속도에 따른 움직임 상태 업데이트
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

            // 이동 입력이 있을 때만 isMoving을 true로 설정
            isMoving = moveDirection.magnitude > 0.1f;

            // 이동 중일 때 상태 업데이트
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

            // 이동 속도 적용 및 수직 속도 유지
            moveDirection *= currentSpeed;
            moveDirection.y = moveDirectionY;

            // 중력 적용
            ySpeed += gravity * Time.deltaTime;
            moveDirection.y = ySpeed;

            // 계산된 방향과 속도로 플레이어 이동
            controller.Move(moveDirection * Time.deltaTime);
        }

        // 웅크리기 로직 처리
        private void HandleCrouch()
        {
            if (Input.GetKeyDown(crouchKey))
            {
                if (Input.GetKey(runKey) && !isCrouching && !isSliding && !isProning)
                {
                    StartSlide();  // 달리는 중에 웅크리기 버튼 누르면 슬라이딩 시작
                }
                else
                {
                    if (isProning)
                    {
                        // 엎드린 상태에서 웅크리기로 전환
                        isProning = false;
                        isCrouching = true;
                        currentSpeed = crouchSpeed;
                        controller.height = crouchHeight;
                        targetCameraPosition = new Vector3(
                            playerCamera.transform.localPosition.x,
                            cameraInitialPosition.y - crouchHeight,
                            playerCamera.transform.localPosition.z
                        );
                    }
                    else
                    {
                        // 웅크리기 토글
                        isCrouching = !isCrouching;
                        if (isCrouching)
                        {
                            currentSpeed = crouchSpeed;
                            controller.height = crouchHeight;
                            targetCameraPosition = new Vector3(
                                playerCamera.transform.localPosition.x,
                                cameraInitialPosition.y - crouchHeight,
                                playerCamera.transform.localPosition.z
                            );
                        }
                        else
                        {
                            currentSpeed = Input.GetKey(runKey) ? runSpeed : walkSpeed;
                            controller.height = standHeight;
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

        // 엎드리기 로직 처리
        private void HandleProne()
        {
            if (Input.GetKeyDown(proneKey))
            {
                if (isCrouching)
                {
                    // 웅크린 상태에서 엎드리기로 전환
                    isCrouching = false;
                    isProning = true;
                    currentSpeed = proneSpeed;
                    controller.height = proneHeight;
                    targetCameraPosition = new Vector3(
                        playerCamera.transform.localPosition.x,
                        cameraInitialPosition.y - proneHeight,
                        playerCamera.transform.localPosition.z
                    );
                }
                else
                {
                    // 엎드리기 토글
                    isProning = !isProning;
                    if (isProning)
                    {
                        currentSpeed = proneSpeed;
                        controller.height = proneHeight;
                        targetCameraPosition = new Vector3(
                            playerCamera.transform.localPosition.x,
                            cameraInitialPosition.y - proneHeight,
                            playerCamera.transform.localPosition.z
                        );
                    }
                    else
                    {
                        currentSpeed = Input.GetKey(runKey) ? runSpeed : walkSpeed;
                        controller.height = standHeight;
                        targetCameraPosition = new Vector3(
                            playerCamera.transform.localPosition.x,
                            cameraInitialPosition.y,
                            playerCamera.transform.localPosition.z
                        );
                    }
                }
            }
        }

        // 점프 로직 처리
        private void HandleJump()
        {
            if (Input.GetKeyDown(jumpKey) && controller.isGrounded && !isSliding && !isProning && !isCrouching)
            {
                ySpeed = jumpForce;  // 점프 힘 적용
            }
        }

        // 슬라이딩 시작
        private void StartSlide()
        {
            isSliding = true;
            currentSpeed = slideSpeed;
            slideTimer = slideDuration;
            isCrouching = true;
            isProning = false;
            controller.height = crouchHeight;
            targetCameraPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                cameraInitialPosition.y - slideHeight,
                playerCamera.transform.localPosition.z
            );
        }

        // 슬라이딩 이동 및 타이머 처리
        private void HandleSlide()
        {
            controller.Move(transform.forward * slideSpeed * Time.deltaTime);
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                EndSlide();
            }
        }

        // 슬라이딩 종료 및 플레이어 상태 리셋
        private void EndSlide()
        {
            isSliding = false;
            isCrouching = false;
            isProning = false;
            currentSpeed = walkSpeed;
            controller.height = standHeight;
            targetCameraPosition = cameraInitialPosition;
        }
    }
}