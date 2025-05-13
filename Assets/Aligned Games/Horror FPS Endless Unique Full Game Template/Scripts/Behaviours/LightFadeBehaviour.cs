using UnityEngine;

namespace AlignedGames
{
    public class LightFadeBehaviour : MonoBehaviour
    {
        [SerializeField] private Light targetLight;       // The light to fade in/out
        [SerializeField] private float fadeDuration;      // Duration of the fade effect
        [SerializeField] private float maxIntensity;      // Maximum intensity of the light

        private float timer = 0f;                          // Timer to track the fade progress
        private bool fadingIn = true;                      // Flag to determine if the light is fading in or out

        void Start()
        {
            // If no target light is assigned, get the Light component attached to this GameObject
            if (targetLight == null)
            {
                targetLight = GetComponent<Light>();
            }
            // Start with the light intensity set to 0 (off)
            targetLight.intensity = 0f;
        }

        void Update()
        {
            // Increase the timer by the amount of time passed since the last frame
            timer += Time.deltaTime;
            // Calculate the progress of the fade effect as a percentage (0 to 1)
            float progress = timer / fadeDuration;

            if (fadingIn)
            {
                // Fade in by lerping from 0 intensity to the maximum intensity
                targetLight.intensity = Mathf.Lerp(0f, maxIntensity, progress);
                if (progress >= 1f)
                {
                    // Once the fade-in is complete, switch to fading out
                    fadingIn = false;
                    timer = 0f; // Reset the timer for the fade-out
                }
            }
            else
            {
                // Fade out by lerping from the maximum intensity to 0 intensity
                targetLight.intensity = Mathf.Lerp(maxIntensity, 0f, progress);
                if (progress >= 1f)
                {
                    // Once the fade-out is complete, destroy this GameObject (i.e., the light)
                    Destroy(gameObject);
                }
            }
        }
    }
}