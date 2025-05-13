using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class DestroyAtDeathBehaviour : MonoBehaviour
    {
        public bool DestroyCollider; // Boolean flag to determine whether to destroy the collider

        public void OnDestroy()
        {
            if (DestroyCollider) // Check if collider destruction is enabled
            {
                Destroy(GetComponent<Collider>()); // Destroy the Collider component attached to this GameObject
            }
        }
    }
}
