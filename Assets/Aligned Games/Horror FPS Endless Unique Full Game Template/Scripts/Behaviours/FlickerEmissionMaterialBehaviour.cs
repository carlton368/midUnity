using UnityEngine;

namespace AlignedGames
{
    public class FlickerEmissionMaterialBehaviour : MonoBehaviour
    {
        public Material targetMaterial; // The material to apply the emission effect (if assigned)
        public Color emissionColor = Color.white; // The base color of the emission
        public float pulseSpeed = 2f; // Speed at which the emission pulses
        public float minEmission = 0.1f; // Minimum intensity of the emission
        public float maxEmission = 1f; // Maximum intensity of the emission

        private Material instanceMaterial; // Instance material to avoid modifying shared materials
        private float pulseTimer; // Timer to track the pulsing effect

        void Start()
        {
            // If no material is assigned, create an instance from the object's material
            if (targetMaterial == null)
            {
                Renderer renderer = GetComponent<Renderer>(); // Get the Renderer component
                if (renderer != null)
                {
                    instanceMaterial = new Material(renderer.sharedMaterial); // Duplicate the shared material
                    renderer.material = instanceMaterial; // Apply the instance material to avoid modifying shared materials
                }
                else
                {
                    Debug.LogError("Renderer component not found."); // Log an error if no renderer exists
                    return;
                }
            }
            else
            {
                instanceMaterial = targetMaterial; // Use the assigned material
            }

            instanceMaterial.EnableKeyword("_EMISSION"); // Enable emission on the material
        }

        void Update()
        {
            // Create a pulsing effect using a sine wave
            pulseTimer += Time.deltaTime * pulseSpeed;
            float emissionIntensity = Mathf.Lerp(minEmission, maxEmission, (Mathf.Sin(pulseTimer) + 1f) / 2f);

            // Calculate the final emission color based on intensity
            Color finalEmissionColor = emissionColor * emissionIntensity;
            instanceMaterial.SetColor("_EmissionColor", finalEmissionColor); // Apply the emission color to the material
        }
    }
}
