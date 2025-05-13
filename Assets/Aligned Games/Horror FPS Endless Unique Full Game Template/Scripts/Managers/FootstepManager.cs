using UnityEngine;

namespace AlignedGames

{

    //This script has been altered to work with this package, an archived (different namespace) version of the original package is included in this template.

    public class FootstepManager : MonoBehaviour
    {

        public AudioSource footstepSource;           // AudioSource to play footstep sounds
        public GroundType[] groundTypes;            // Array of ground types with footstep sounds
        public float baseFootstepDelay = 2.5f;      // Time between footsteps at default speed

        private PlayerMovementManager playerMovement; // Reference to PlayerMovementManager
        private float currentFootstepTime;          // Timer to track footstep interval
        private AudioClip[] currentFootstepClips;   // Current footstep clips based on ground type

        private CharacterController controller;  // Reference to the CharacterController

        void Start()
        {
            controller = GetComponent<CharacterController>();  // Get the CharacterController component
            // Get the PlayerMovementManager component
            playerMovement = GetComponent<PlayerMovementManager>();

            // Check if an AudioSource is assigned, if not, add one
            if (footstepSource == null)
            {
                footstepSource = gameObject.AddComponent<AudioSource>();
            }
        }

        void Update()
        {
            // Play footsteps if the player is moving and grounded
            if (playerMovement.isMoving && controller.isGrounded)
            {
                UpdateGroundType();
                PlayFootsteps();
                AdjustAudioVolume();
            }
            else
            {
                currentFootstepTime = 0f; // Reset timer when idle or in air
            }
        }

        void UpdateGroundType()
        {
            RaycastHit hit;

            // Perform a raycast to detect the ground below the player
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
            {
                // Get the ground layer
                int groundLayer = hit.collider.gameObject.layer;

                // Find the matching ground type
                foreach (GroundType groundType in groundTypes)
                {
                    if (groundType.groundLayer == (groundType.groundLayer & (1 << groundLayer)))
                    {
                        currentFootstepClips = groundType.footstepClips;
                        return;
                    }
                }
            }

            // Default to the first ground type if no match is found
            if (groundTypes.Length > 0)
            {
                currentFootstepClips = groundTypes[0].footstepClips;
            }
        }

        void PlayFootsteps()
        {
            // Increment footstep timer
            currentFootstepTime += Time.deltaTime;

            // Calculate the delay between footsteps based on player's current movement speed
            float currentSpeed = playerMovement.isRunning ? playerMovement.runSpeed : playerMovement.walkSpeed;
            float adjustedFootstepDelay = baseFootstepDelay / currentSpeed;

            // Play footstep sound if enough time has passed
            if (currentFootstepTime >= adjustedFootstepDelay)
            {
                if (currentFootstepClips != null && currentFootstepClips.Length > 0)
                {
                    footstepSource.clip = GetRandomFootstep();
                    footstepSource.Play();
                }

                currentFootstepTime = 0f; // Reset timer
            }
        }

        public void AdjustAudioVolume()

        {

            // Check if the player is crouching
            if (playerMovement.isCrouching)
            {
                footstepSource.volume = 0.3f;
            }
            // Check if the player is proning
            if (playerMovement.isProning)
            {
                footstepSource.volume = 0.15f;
            }
            // Check if the player is walking or running
            if (!playerMovement.isProning && !playerMovement.isCrouching)
            {
                footstepSource.volume = 0.5f;  // Reset volume to normal
            }
        }

        AudioClip GetRandomFootstep()
        {
            // Return a random footstep clip from the current array
            return currentFootstepClips[Random.Range(0, currentFootstepClips.Length)];
        }
    }

    [System.Serializable]
    public class GroundType
    {
        public string name;                 // Name of the ground type (for easy identification)
        public LayerMask groundLayer;       // LayerMask to identify this ground type
        public AudioClip[] footstepClips;   // Array of footstep sounds for this ground type
    }

}