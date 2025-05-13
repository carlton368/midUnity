using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class ForceBehaviour : MonoBehaviour
    {
        public float Force; // Force magnitude to be applied

        void Start()
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * Force); // Apply force in the forward direction of the GameObject
        }
    }
}
