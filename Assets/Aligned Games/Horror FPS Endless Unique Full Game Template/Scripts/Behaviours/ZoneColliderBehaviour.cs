using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class ZoneColliderBehaviour : MonoBehaviour
    {
        // Booleans to determine if the zone deals damage or heals
        public bool DamageZone;
        public bool HealZone;

        // Amount of damage or healing applied
        public float DamageAmount;
        public float HealAmount;

        // Divisor for damage calculation
        public float DivideAmount;

        // Trigger event when an object enters the collider
        public void OnTriggerEnter(Collider other)
        {
            // Check if the colliding object is the player
            if (other.gameObject.tag == "Player")
            {
                // Apply damage if this is a damage zone
                if (DamageZone)
                {
                    other.gameObject.GetComponent<PlayerVitalsManager>().Health -= DamageAmount / DivideAmount;
                }

                // Apply healing if this is a healing zone
                if (HealZone)
                {
                    other.gameObject.GetComponent<PlayerVitalsManager>().Health += DamageAmount;
                }
            }

            // Check if the colliding object is an enemy
            if (other.gameObject.tag == "Enemy")
            {
                // Apply damage if this is a damage zone and the enemy has an EnemyHealthManager component
                if (DamageZone)
                {
                    if (other.gameObject.GetComponent<EnemyHealthManager>())
                    {
                        other.gameObject.GetComponent<EnemyHealthManager>().Health -= DamageAmount;
                    }
                }

                // Apply healing if this is a healing zone and the enemy has an EnemyHealthManager component
                if (HealZone)
                {
                    if (other.gameObject.GetComponent<EnemyHealthManager>())
                    {
                        other.gameObject.GetComponent<EnemyHealthManager>().Health += DamageAmount;
                    }
                }
            }
        }
    }
}
