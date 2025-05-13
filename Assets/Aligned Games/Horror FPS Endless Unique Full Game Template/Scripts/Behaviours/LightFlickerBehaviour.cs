using UnityEngine;

namespace AlignedGames
{
#if UNITY_EDITOR
    using UnityEditor; // Include UnityEditor for editor-specific functionality
#endif

    namespace AlignedGames
    {
        public class LightFlicker : MonoBehaviour
        {
            public float maxFactor = 1.2f;  // Maximum intensity multiplier for the light
            public float minFactor = 1.0f;  // Minimum intensity multiplier for the light
            public float moveRange = 0.1f;  // The range within which the light can move
            public float speed = 0.1f;      // The speed at which the flickering happens

            private float currentFactor = 1.0f;  // Current intensity factor for the light
            private Vector3 currentPos;          // Current position of the light
            private float deltaTime;             // Delta time for frame updates
            private Vector3 initPos;             // Initial position of the light
            private float targetFactor;          // Target intensity factor to lerp towards
            private Vector3 targetPos;           // Target position to lerp towards
            private float initialFactor;         // Initial intensity factor of the light
            private float time;                  // Tracks time for editor-based update
            private float timeLeft;              // Time left before next flicker update

            private void Start()
            {
                // Initialize random state based on the object's position
                Random.InitState((int)transform.position.x + (int)transform.position.y);
                // Get the initial intensity of the light when the script starts
                initialFactor = GetComponent<Light>().intensity;
            }

            private void OnEnable()
            {
                // Set the initial position of the light when the object is enabled
                initPos = transform.localPosition;
                currentPos = initPos;
            }

            private void OnDisable()
            {
                // Reset the light's position to the initial position when the object is disabled
                transform.localPosition = initPos;
            }

#if !UNITY_EDITOR
            private void Update()
            {
                deltaTime = Time.deltaTime; // Get the time passed since the last frame
#else
            void OnRenderObject()
            {
                // Editor-specific: calculate time delta based on Unity's editor time
                float currentTime = (float)EditorApplication.timeSinceStartup;
                deltaTime = currentTime - time;
                time = currentTime;
#endif

                // If timeLeft is less than or equal to deltaTime, update the flicker values
                if (timeLeft <= deltaTime)
                {
                    // Set the new target intensity factor and position
                    targetFactor = Random.Range(minFactor, maxFactor); // Random intensity factor
                    targetPos = initPos + Random.insideUnitSphere * moveRange; // Random position within the move range
                    timeLeft = speed; // Reset the time left to the speed value
                }
                else
                {
                    // Lerp the intensity factor and position towards their targets
                    float weight = deltaTime / timeLeft; // Calculate weight for lerping based on time remaining
                    currentFactor = Mathf.Lerp(currentFactor, targetFactor, weight); // Interpolate light intensity
                    GetComponent<Light>().intensity = initialFactor * currentFactor; // Apply the new intensity
                    currentPos = Vector3.Lerp(currentPos, targetPos, weight); // Interpolate position
                    transform.localPosition = currentPos; // Apply the new position
                    timeLeft -= deltaTime; // Decrease the time left by deltaTime
                }
            }
        }
    }
}
