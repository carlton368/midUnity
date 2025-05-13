using UnityEngine;

namespace AlignedGames
{
    public class ScreenShakeManager : MonoBehaviour
    {
        [Header("Shake Parameters")]
        public float shakeDuration; // Duration of the shake effect
        public float shakeMagnitude; // Intensity of the shake
        public float dampingSpeed; // Speed at which the shake decreases over time

        private float currentShakeDuration; // Tracks the remaining shake duration

        void Update()
        {
            // If the shake is ongoing (i.e., currentShakeDuration is greater than 0)
            if (currentShakeDuration > 0)
            {
                // Apply a random displacement to the camera's local position to simulate the shake
                transform.localPosition = transform.localPosition + Random.insideUnitSphere * shakeMagnitude;

                // Reduce the remaining shake duration based on the damping speed
                currentShakeDuration -= Time.deltaTime * dampingSpeed;
            }
            else
            {
                // Reset shake duration when finished
                currentShakeDuration = 0f;
            }
        }

        // Method to trigger the screen shake effect with custom duration and magnitude
        public void TriggerShake(float duration, float magnitude)
        {
            // If the player is not aiming, use the full shake parameters
            if (!GetComponentInChildren<WeaponAnimatorBehaviour>().isAiming)
            {
                shakeDuration = duration;
                shakeMagnitude = magnitude;
                currentShakeDuration = shakeDuration;
            }
            else
            {
                // If the player is aiming, reduce the shake's intensity and duration
                shakeDuration = duration / 2;
                shakeMagnitude = magnitude / 2;
                currentShakeDuration = shakeDuration / 2;
            }
        }

        // This method responds to trauma events (like explosions or damage), triggering a shake effect
        public void OnReceiveTrauma(float traumaDuration, float traumaMagnitude)
        {
            TriggerShake(traumaDuration, traumaMagnitude);
        }
    }
}
