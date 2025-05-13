using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlignedGames
{
    // This class manages the health and death of an enemy, including damage application, healing, and ragdoll effects
    public class EnemyHealthManager : MonoBehaviour
    {
        // The current health of the enemy
        public float Health;
        // The maximum health the enemy can have
        public float MaxHealth;
        // The minimum health the enemy can have (used for limiting health)
        public float MinHealth;
        // Reference to the player object
        public GameObject Player;
        // Reference to the ragdoll prefab that will be instantiated on death
        public GameObject Ragdoll;

        // A boolean to track whether the enemy's kill count has been added
        public bool HasAddedKill;

        // A boolean that indicates if the enemy can currently take damage
        public bool CanTakeDamage;

        // Reference to the scene manager object, used to manage game state (e.g., score)
        public GameObject SceneManager;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Find the player object in the scene using its "Player" tag
            Player = GameObject.FindWithTag("Player");

            // Find the SceneManager object by name
            SceneManager = GameObject.Find("SceneManager");
        }

        // Update is called once per frame
        private void Update()
        {
            // Check if the enemy's health has dropped to zero or below
            if (Health <= 0)
            {
                // Instantiate the ragdoll effect at the enemy's position and rotation
                Ragdoll = Instantiate(Ragdoll, transform.position, transform.rotation);
                // Call the Death function to handle enemy death
                Death();
            }
        }

        // This function applies damage to the enemy's health if it can take damage
        public void ApplyDamage(float Damage)
        {
            // Only apply damage if the enemy is allowed to take damage
            if (CanTakeDamage)
            {
                // Decrease health by the amount of damage
                Health -= Damage;
            }
        }

        // This function applies healing to the enemy's health
        public void ApplyHealing(int HealAmount)
        {
            // Increase health by the healing amount
            Health += HealAmount;
        }

        // This function applies explosion damage to the enemy's health
        public void ApplyExplosion(int Damage)
        {
            // Decrease health by the explosion damage
            Health -= Damage;
        }

        // This function handles the death of the enemy
        public void Death()
        {
            // Check if the kill has already been added (to avoid multiple additions)
            if (!HasAddedKill)
            {
                // Set HasAddedKill to true to avoid double counting
                HasAddedKill = true;

                // Increase the kill count in the ScoreManager
                SceneManager.GetComponent<ScoreManager>().KillsAmount++;

                // Destroy the enemy object (it is considered dead now)
                Destroy(this.gameObject);
            }
        }
    }
}
