using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class DoorTriggerBehaviour : MonoBehaviour
    {
        public GameObject DoorToOpen; // Reference to the door GameObject to be opened/closed
        public bool DoorOpen; // Tracks whether the door is currently open
        public bool IsDoor1; // Determines if this script controls Door 1
        public bool IsDoor2; // Determines if this script controls Door 2

        public void OnTriggerEnter(Collider other)
        {
            // Check if the object that entered the trigger has the "Player" tag
            if (other.tag == "Player")
            {
                // If the door is not open and the animation is not already playing
                if (!DoorOpen)
                {
                    if (DoorToOpen.GetComponent<Animation>().isPlaying == false)
                    {
                        // If this is Door 1, play the opening animation for Door 1
                        if (IsDoor1)
                        {
                            DoorToOpen.GetComponent<Animation>().Play("door_1_open_1");
                            DoorOpen = true; // Mark the door as open
                        }

                        // If this is Door 2, play the opening animation for Door 2
                        if (IsDoor2)
                        {
                            DoorToOpen.GetComponent<Animation>().Play("door_2_open_1");
                            DoorOpen = true; // Mark the door as open
                        }
                    }
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            // Check if the object that exited the trigger has the "Player" tag
            if (other.tag == "Player")
            {
                // If the door is open and the animation is not already playing
                if (DoorOpen)
                {
                    if (DoorToOpen.GetComponent<Animation>().isPlaying == false)
                    {
                        // If this is Door 1, play the closing animation for Door 1
                        if (IsDoor1)
                        {
                            DoorToOpen.GetComponent<Animation>().Play("door_1_close_1");
                            DoorOpen = false; // Mark the door as closed
                        }

                        // If this is Door 2, play the closing animation for Door 2
                        if (IsDoor2)
                        {
                            DoorToOpen.GetComponent<Animation>().Play("door_2_close_1");
                            DoorOpen = false; // Mark the door as closed
                        }
                    }
                }
            }
        }
    }
}
