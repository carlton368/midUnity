using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    // This class handles the behavior of a projectile in the game
    public class ProjectileBehaviour : MonoBehaviour
    {
        // The speed at which the projectile moves
        public float Speed;
        // The amount of damage the projectile deals
        public float Damage;
        // The explosion effect that occurs when the projectile hits something
        public GameObject Explosion;
        // An array of tags that should be ignored when checking for collisions
        public string[] ignoredTags;

        // Update is called once per frame
        void Update()
        {
            // Move the projectile forward based on its speed, in the direction of its local forward axis
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        }

        // This function is called when the projectile collides with another collider
        public void OnTriggerEnter(Collider other)
        {
            // Iterate through each tag in the ignoredTags array
            foreach (string tag in ignoredTags)
            {
                // If the collider's tag matches any of the ignored tags, do nothing and return
                if (other.CompareTag(tag))
                {
                    return;
                }
            }

            // If the collider's tag is "Player", apply damage and trigger the explosion effect
            if (other.transform.gameObject.tag == "Player")
            {
                // Send a message to the PlayerVitalsManager component to apply the damage to the player
                other.GetComponent<PlayerVitalsManager>().SendMessage("ApplyDamage", Damage);
                // Instantiate the explosion at the projectile's current position and rotation
                Instantiate(Explosion, transform.position, transform.rotation);
                // Destroy the projectile object after it hits the player
                Destroy(this.gameObject);
            }

            // If the collider's tag is "Untagged", trigger the explosion and destroy the projectile
            if (other.transform.gameObject.tag == "Untagged")
            {
                // Instantiate the explosion at the projectile's current position and rotation
                Instantiate(Explosion, transform.position, transform.rotation);
                // Destroy the projectile object after it hits an untagged object
                Destroy(this.gameObject);
            }
        }
    }
}
