using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class ObjectHealthManager : MonoBehaviour
    {
        // The health of the object
        public float Health;

        // Flag to determine if the object is explosive
        public bool IsExplosive;

        // Flag to determine if the object can drop pickups
        public bool IsPickups;

        // Explosion effect to instantiate when the object is destroyed
        public GameObject PiecesExplosion;

        // Pickup that the object will drop when destroyed
        public GameObject Pickup;

        // Array of possible pickups that can be dropped
        public GameObject[] Pickups;

        // Update is called once per frame
        void Update()
        {
            // Check if the object's health is less than or equal to 0
            if (Health <= 0)
            {
                // If the object is explosive, instantiate the explosion effect
                if (IsExplosive)
                {
                    Instantiate(PiecesExplosion, transform.position, transform.rotation);

                    // If the object can drop pickups, instantiate a random pickup
                    if (IsPickups)
                    {
                        Pickup = Instantiate(Pickups[Random.Range(0, Pickups.Length)], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                    }
                }

                // Destroy the object
                Destroy(this.gameObject);
            }
        }

        // Method to apply damage to the object
        public void ApplyDamage(float Damage)
        {
            Health -= Damage; // Decrease health by the damage amount
        }

    }

}