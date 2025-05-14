using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AlignedGames
{
    // This class manages the player's health, damage application, and the UI for health.
    public class PlayerVitalsManager : MonoBehaviour
    {
        // Player's current health and maximum health.
        public float Health;
        public float MaxHealth;

        // UI element to display health.
        public GameObject HealthText;

        // Array of audio clips for random damage sounds.
        public AudioClip[] RandomTakeDamageSound;

        // References to the scene manager, weapons, interface, and main camera.
        public GameObject SceneManager;
        public GameObject Weapons;
        public GameObject Interface;
        public GameObject MainCamera;

        // Called when the script is first initialized.
        public void Start()
        {
            // Find the references for the scene manager and the main camera at the start.
            SceneManager = GameObject.Find("SceneManager");
            MainCamera = GameObject.FindWithTag("MainCamera");
        }

        // Update is called once per frame.
        public void Update()
        {
            // Update the health UI text.
//            HealthText = GameObject.Find("HealthText");
//            HealthText.GetComponent<Text>().text = Health.ToString("0");

            // Check if the game is not paused.
            if (Time.timeScale == 1)
            {
                // If health is less than or equal to 0, the player dies.
                if (Health <= 0)
                {
                    Health = 0; // Ensure health doesn't go below zero.

                    // Disable the player's camera manager, movement manager, and weapons.
                    GetComponentInChildren<PlayerCameraManager>().enabled = false;
                    GetComponent<PlayerMovementManager>().enabled = false;
                    Weapons.SetActive(false);
                    Interface.SetActive(false);

                    // Play the death animation on the camera.
                    MainCamera.GetComponent<Animation>().Play("Death");

                    // Show the end menu after a delay (3 seconds).
                    Invoke("ShowEndMenu", 3f);
                }

                // Ensure health doesn't exceed the maximum value.
                if (Health > MaxHealth)
                {
                    Health = MaxHealth;
                }
            }
        }

        // Shows the end menu when the player dies.
        public void ShowEndMenu()
        {
            // Activate the end menu through the scene manager.
            SceneManager.GetComponent<MenuManager>().ShowEndMenu();

            // Pause the game.
            Time.timeScale = 0;
        }

        // Apply damage to the player.
        public void ApplyDamage(int Damage)
        {
            // Subtract the damage from the player's health.
            Health -= Damage;

            // Play a sound when the player takes damage.
            PlayTakeDamageSound();
        }

        // Play a random damage sound when the player takes damage.
        public void PlayTakeDamageSound()
        {
            // Only play a sound if one isn't already playing.
            if (!GetComponent<AudioSource>().isPlaying)
            {
                // Pick a random sound from the damage sounds array.
                GetComponent<AudioSource>().clip = RandomTakeDamageSound[Random.Range(0, RandomTakeDamageSound.Length)];

                // Play the selected sound.
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
