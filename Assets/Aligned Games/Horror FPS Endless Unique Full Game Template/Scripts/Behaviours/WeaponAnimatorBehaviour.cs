using UnityEngine;

namespace AlignedGames
{
    public class WeaponAnimatorBehaviour : MonoBehaviour
    {
        // References to components for the first-person arms, movement, and checking movement status
        public Transform fpsArms; // Reference to the FPS arms
        public PlayerMovementManager movementScript; // Reference to the movement check script
        public CheckMovementBehaviour checkmovementScript; // Reference to the movement check script

        // Variables to control sway effects during various states
        public float swayAmount = 0.05f; // Amount of sway during normal movement
        public float aimSwayAmount = 0.01f; // Amount of sway while aiming

        // Speed settings for sway effects
        public float swaySpeed = 2f; // Speed of sway
        public float SlowswaySpeed = 2f; // Sway speed when walking
        public float NormalswaySpeed = 2f; // Sway speed when idle
        public float QuickswaySpeed = 2f; // Sway speed when running
        public float aimswaySpeed = 0f; // Sway speed while aiming

        public float transitionSpeed = 5f; // Speed of transition between states

        // Internal variables to store initial and target position/rotation of the weapon
        private Vector3 initialPosition; // Initial position of the FPS arms
        private Quaternion initialRotation; // Initial rotation of the FPS arms
        private Vector3 currentPosition; // Current position from WeaponAimBehaviour
        private Vector3 targetPosition; // Target position based on sway animation
        private Quaternion targetRotation; // Target rotation based on sway animation

        public bool isAiming = false; // Tracks whether the player is aiming

        void Start()
        {
            // Initialize the starting positions and rotations
            initialPosition = fpsArms.localPosition;
            initialRotation = fpsArms.localRotation;
            targetPosition = initialPosition;
            targetRotation = initialRotation;
            currentPosition = initialPosition; // Set current position to initial at start
        }

        void Update()
        {
            // Ensure the sway animation only runs when time is progressing
            if (Time.deltaTime != 0)
            {
                // Determine the sway animation based on movement and aiming status
                if (checkmovementScript.IsMoving)
                {
                    // Handle different sway behavior based on movement speed
                    if (movementScript.currentSpeed != movementScript.runSpeed)
                    {
                        if (isAiming)
                            AnimateAimSway(); // Apply aiming sway
                        else
                            AnimateSwaySlow(); // Apply slow sway for walking
                    }
                    else
                    {
                        if (isAiming)
                            AnimateAimSway(); // Apply aiming sway when running
                        else
                            AnimateSwayQuick(); // Apply quick sway for running
                    }
                }
                else
                {
                    // Apply idle sway or aiming sway when not moving
                    if (isAiming)
                        AnimateAimSway(); // Apply aiming sway
                    else
                        AnimateSwayIdle(); // Apply idle sway
                }

                // Smoothly interpolate to the target position and rotation, adjusting based on current position
                fpsArms.localPosition = Vector3.Lerp(fpsArms.localPosition, targetPosition + currentPosition, Time.deltaTime * transitionSpeed);
                fpsArms.localRotation = Quaternion.Lerp(fpsArms.localRotation, targetRotation, Time.deltaTime * transitionSpeed);
            }
        }

        // Method to update the current position of the weapon
        public void SetCurrentPosition(Vector3 position)
        {
            currentPosition = position;
        }

        // Method to set whether the player is aiming
        public void SetAiming(bool aiming)
        {
            isAiming = aiming; // Set whether the player is aiming
        }

        // Animate sway during slow movement (e.g., walking)
        void AnimateSwaySlow()
        {
            swaySpeed = SlowswaySpeed; // Set sway speed for slow movement
            float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount; // Calculate sway amount based on sine wave
            targetPosition = new Vector3(swayX, 0, 0); // Adjust the sway relative to the current position
            targetRotation = initialRotation * Quaternion.Euler(0, swayX * 5f, 0); // Apply rotation to simulate sway
        }

        // Animate sway during quick movement (e.g., running)
        void AnimateSwayQuick()
        {
            swaySpeed = QuickswaySpeed; // Set sway speed for quick movement
            float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount; // Calculate sway amount based on sine wave
            targetPosition = new Vector3(swayX, 0, 0); // Adjust the sway relative to the current position
            targetRotation = initialRotation * Quaternion.Euler(0, swayX * 5f, 0); // Apply rotation to simulate sway
        }

        // Animate sway when idle (not moving)
        void AnimateSwayIdle()
        {
            swaySpeed = NormalswaySpeed; // Set sway speed for idle state
            float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount / 2; // Reduce sway for idle
            targetPosition = new Vector3(swayX, 0, 0); // Adjust the sway relative to the current position
            targetRotation = initialRotation * Quaternion.Euler(0, swayX * 5f, 0); // Apply rotation to simulate sway
        }

        // Animate sway while aiming (minimal sway)
        void AnimateAimSway()
        {
            swaySpeed = aimswaySpeed; // Set sway speed for aiming
            float swayX = Mathf.Sin(Time.time * swaySpeed) * aimSwayAmount; // Calculate reduced sway during aiming
            targetPosition = new Vector3(swayX, 0, 0); // Apply reduced sway during aiming
            targetRotation = initialRotation * Quaternion.Euler(0, swayX * 5f, 0); // Apply rotation to simulate sway
        }
    }
}
