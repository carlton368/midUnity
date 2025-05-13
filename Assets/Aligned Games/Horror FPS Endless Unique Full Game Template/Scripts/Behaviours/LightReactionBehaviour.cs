using UnityEngine;

namespace AlignedGames
{
    public class LightReactionBehaviour : MonoBehaviour
    {
        public Renderer enemyRenderer; // Reference to the Renderer component of the enemy
        private Material enemyMaterial; // The material of the enemy's renderer
        public bool canTakeDamage = false; // Tracks whether the enemy can take damage
        public Color damageableColor; // Color to indicate that the enemy can take damage
        public Color nonDamageableColor; // Color to indicate that the enemy cannot take damage
        public float fadeDuration = 1f; // Duration of the fade effect

        private Color targetEmissionColor; // Target emission color for fading
        private Color targetAlbedoColor; // Target albedo color for fading
        private Color currentEmissionColor; // Current emission color for lerping
        private Color currentAlbedoColor; // Current albedo color for lerping
        private float fadeTimer = 0f; // Timer to track fade progress

        void Start()
        {
            // Check if the enemyRenderer is assigned
            if (enemyRenderer != null)
            {
                // Get the material from the enemyRenderer to modify it
                enemyMaterial = enemyRenderer.material;

                // Initialize colors
                currentEmissionColor = enemyMaterial.GetColor("_EmissionColor");

                // Check for both _Color and _AlbedoColor to support different shaders
                if (enemyMaterial.HasProperty("_AlbedoColor"))
                {
                    currentAlbedoColor = enemyMaterial.GetColor("_AlbedoColor");
                }
                else
                {
                    currentAlbedoColor = enemyMaterial.GetColor("_Color");
                }

                targetEmissionColor = canTakeDamage ? damageableColor : nonDamageableColor;
                targetAlbedoColor = canTakeDamage ? damageableColor : nonDamageableColor;
            }
            else
            {
                // Log an error if no renderer is found
                Debug.LogError("No Renderer found on the enemy. Make sure the enemy has a material.");
            }
        }

        void Update()
        {
            // Only proceed if enemyMaterial is not null
            if (enemyMaterial != null)
            {
                // Increment fade timer based on delta time
                fadeTimer += Time.deltaTime / fadeDuration;

                // Lerp colors gradually for a smooth transition
                currentEmissionColor = Color.Lerp(currentEmissionColor, targetEmissionColor, fadeTimer);
                currentAlbedoColor = Color.Lerp(currentAlbedoColor, targetAlbedoColor, fadeTimer);

                // Apply the lerped colors to the material
                enemyMaterial.SetColor("_EmissionColor", currentEmissionColor);

                // Apply to the correct property depending on the shader
                if (enemyMaterial.HasProperty("_AlbedoColor"))
                {
                    enemyMaterial.SetColor("_AlbedoColor", currentAlbedoColor);
                }
                else
                {
                    enemyMaterial.SetColor("_Color", currentAlbedoColor);
                }
            }
        }

        // This function is called from other scripts to change the damageable state
        public void SetCanTakeDamage(bool value)
        {
            // Only update if the state actually changes
            if (canTakeDamage != value)
            {
                canTakeDamage = value; // Update state

                // Set target colors based on new state
                targetEmissionColor = canTakeDamage ? damageableColor : nonDamageableColor;
                targetAlbedoColor = canTakeDamage ? damageableColor : nonDamageableColor;

                // Reset fade timer to restart transition
                fadeTimer = 0f;
            }
        }
    }
}
