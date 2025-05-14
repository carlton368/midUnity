using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class InterfaceAnimationManager : MonoBehaviour
    {
        // Reference to the Crosshair object to animate
        public GameObject Crosshair;

        // Reference to the Player object to check if it's moving
        public GameObject Player;

        // Update is called once per frame
        void Update()
        {
            // Check if the player is moving using the IsMoving property of the CheckMovementBehaviour script
            if (Player.GetComponent<CheckMovementBehaviour>().IsMoving)
            {
                // If the player is moving, play the animation on the crosshair
                // Crosshair.GetComponent<Animation>().Play();
            }
            else
            {
                // If the player is not moving, stop the animation on the crosshair
                // Crosshair.GetComponent<Animation>().Stop();
            }
        }
    }
}
