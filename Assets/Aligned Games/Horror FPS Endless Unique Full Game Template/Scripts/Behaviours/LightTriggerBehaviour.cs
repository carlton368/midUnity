using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class LightTriggerBehaviour : MonoBehaviour
    {
        public float lightRadius = 5f;  // Distance within which enemies are affected by the light
        public string enemyTag = "Enemy"; // Tag used to identify enemies (e.g., "Enemy")

        public List<Collider> detectedEnemies = new List<Collider>(); // List to store enemies currently within the light range

        void Update()
        {
            // Continuously check for enemies within the light radius
            CheckEnemiesInLight();
        }

        void CheckEnemiesInLight()
        {
            // Clear the previously detected enemies from the last frame
            detectedEnemies.Clear();

            // Find all colliders within the specified radius of the light source
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, lightRadius);

            // Iterate through all detected colliders in the range
            foreach (Collider enemy in enemiesInRange)
            {
                if (enemy.CompareTag(enemyTag))  // Check if the collider is tagged as "Enemy"
                {
                    detectedEnemies.Add(enemy);  // Add the enemy to the detected list

                    // Make the enemy damageable (e.g., change color to grey)
                    LightReactionBehaviour lightReaction = enemy.GetComponent<LightReactionBehaviour>();
                    if (lightReaction != null)
                    {
                        lightReaction.SetCanTakeDamage(true);  // Enable damage for the enemy
                    }

                    // Mark the enemy as damageable (increasing health or triggering effects)
                    EnemyHealthManager health = enemy.GetComponent<EnemyHealthManager>();
                    if (health != null)
                    {
                        health.CanTakeDamage = true;  // Enable damage to the enemy
                    }
                }
            }

            // Iterate through previously detected enemies and mark them as non-damageable if they are no longer in range
            foreach (Collider enemy in detectedEnemies)
            {
                if (!IsEnemyInRange(enemy))  // If the enemy is out of range
                {
                    // Set the enemy to be non-damageable (e.g., change color to black)
                    LightReactionBehaviour lightReaction = enemy.GetComponent<LightReactionBehaviour>();
                    if (lightReaction != null)
                    {
                        lightReaction.SetCanTakeDamage(false);  // Disable damage for the enemy
                    }

                    // Mark the enemy as non-damageable (disable health-related effects)
                    EnemyHealthManager health = enemy.GetComponent<EnemyHealthManager>();
                    if (health != null)
                    {
                        health.CanTakeDamage = false;  // Disable damage to the enemy
                    }
                }
            }
        }

        // Called when the light object is destroyed
        void OnDestroy()
        {
            // Mark all previously detected enemies as non-damageable
            foreach (Collider enemy in detectedEnemies)
            {
                if (enemy != null)
                {
                    // Set the enemy to be non-damageable when the light is destroyed
                    LightReactionBehaviour lightReaction = enemy.GetComponent<LightReactionBehaviour>();
                    if (lightReaction != null)
                    {
                        lightReaction.SetCanTakeDamage(false);  // Disable damage for the enemy
                    }

                    // Mark the enemy as non-damageable
                    EnemyHealthManager health = enemy.GetComponent<EnemyHealthManager>();
                    if (health != null)
                    {
                        health.CanTakeDamage = false;  // Disable damage to the enemy
                    }
                }
            }
        }

        // Called when the light object is disabled (or before it is destroyed)
        void OnDisable()
        {
            // Mark all previously detected enemies as non-damageable
            foreach (Collider enemy in detectedEnemies)
            {
                if (enemy != null)
                {
                    // Set the enemy to be non-damageable when the light is disabled
                    LightReactionBehaviour lightReaction = enemy.GetComponent<LightReactionBehaviour>();
                    if (lightReaction != null)
                    {
                        lightReaction.SetCanTakeDamage(false);  // Disable damage for the enemy
                    }

                    // Mark the enemy as non-damageable
                    EnemyHealthManager health = enemy.GetComponent<EnemyHealthManager>();
                    if (health != null)
                    {
                        health.CanTakeDamage = false;  // Disable damage to the enemy
                    }
                }
            }
        }

        // Helper method to check if an enemy is within the radius of the light
        bool IsEnemyInRange(Collider enemy)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position); // Calculate the distance to the enemy
            return distance <= lightRadius;  // Return true if the enemy is within range
        }

        // Visualize the light radius in the editor for debugging purposes
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;  // Set the color for the Gizmo
            Gizmos.DrawWireSphere(transform.position, lightRadius);  // Draw a wireframe sphere to visualize the radius
        }
    }
}
