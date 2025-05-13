using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    // This class handles playing various audio clips related to the enemy's actions
    public class EnemyAudioManager : MonoBehaviour
    {
        // The sound that plays when the enemy starts chasing
        public AudioClip[] EnemyChaseSounds;
        // The sound that plays when the enemy attacks
        public AudioClip[] EnemyAttackSounds;
        // The sound that plays when the enemy flinches
        public AudioClip[] EnemyFlinchSounds;

        // This function plays the chase sound if it is not already playing
        public void PlayEnemyChaseSound()
        {
            // Check if the audio source is not already playing any sound
            if (!GetComponent<AudioSource>().isPlaying)
            {
                // Stop the audio source if it's playing
                GetComponent<AudioSource>().Stop();
                // Play the chase sound clip
                // Select a random sound from the list
                int randomIndex = Random.Range(0, EnemyChaseSounds.Length); // Random index within the bounds of the sound array
                AudioClip selectedClip = EnemyChaseSounds[randomIndex]; // Get the selected random sound clip

                // Play the selected sound using PlayOneShot to play it once
                GetComponent<AudioSource>().PlayOneShot(selectedClip);

            }
        }

        // This function plays the attack sound if it is not already playing
        public void PlayEnemyAttackSound()
        {
            // Check if the audio source is not already playing any sound
            if (!GetComponent<AudioSource>().isPlaying)
            {
                // Stop the audio source if it's playing
                GetComponent<AudioSource>().Stop();
                // Play the attack sound clip
                int randomIndex = Random.Range(0, EnemyAttackSounds.Length); // Random index within the bounds of the sound array
                AudioClip selectedClip = EnemyAttackSounds[randomIndex]; // Get the selected random sound clip

                // Play the selected sound using PlayOneShot to play it once
                GetComponent<AudioSource>().PlayOneShot(selectedClip);
            }
        }

        // This function plays the flinch sound if it is not already playing
        public void PlayEnemyFlinchSound()
        {
            // Stop the audio source if it's playing
            GetComponent<AudioSource>().Stop();
            // Play the attack sound clip
            int randomIndex = Random.Range(0, EnemyFlinchSounds.Length); // Random index within the bounds of the sound array
            AudioClip selectedClip = EnemyFlinchSounds[randomIndex]; // Get the selected random sound clip

            // Play the selected sound using PlayOneShot to play it once
            GetComponent<AudioSource>().PlayOneShot(selectedClip);
        }
    }

}