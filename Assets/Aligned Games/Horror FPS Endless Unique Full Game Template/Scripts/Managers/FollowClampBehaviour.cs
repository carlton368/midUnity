using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    // This script allows an object to follow a target (e.g., player), while optionally keeping its initial rotation or matching the target's rotation.
    public class FollowClampBehaviour : MonoBehaviour
    {
        // Tag of the object to follow (e.g., Player).
        public string targetTag = "Player";

        // Whether to keep the object's initial rotation or match the target's rotation.
        public bool keepInitialRotation = true;

        private Transform target; // Reference to the target object (player).
        private Quaternion initialRotation; // The initial rotation of the object.

        // Start is called before the first frame update
        void Start()
        {
            // Find the target with the specified tag.
            GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
            if (targetObject != null)
            {
                target = targetObject.transform;
            }
            else
            {
                Debug.LogError("Target with tag '" + targetTag + "' not found.");
            }

            // Store the initial rotation of the object if needed.
            if (keepInitialRotation)
            {
                initialRotation = transform.rotation;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the target is found, update the position and rotation.
            if (target != null)
            {
                // Update the object's position to follow the target, but maintain its current y position.
                transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

                // Maintain the initial rotation if specified.
                if (keepInitialRotation)
                {
                    transform.rotation = initialRotation;
                }
                else
                {
                    // Optionally, match the target's rotation if needed.
                    transform.rotation = target.rotation;
                }
            }
        }
    }
}
