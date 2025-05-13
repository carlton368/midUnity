using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class CheckMovementBehaviour : MonoBehaviour
    {
        public Vector3 LastPosition; // Stores the last recorded position of the GameObject
        public bool IsMoving; // Indicates whether the GameObject is currently moving

        void Update()
        {
            if (LastPosition != gameObject.transform.position) // Check if the position has changed since the last frame
            {
                IsMoving = true; // Set to true if the GameObject has moved
            }
            else
            {
                IsMoving = false; // Set to false if the GameObject has not moved
            }

            LastPosition = gameObject.transform.position; // Update LastPosition with the current position
        }
    }
}
