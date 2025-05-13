using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class RandomForceBehaviour : MonoBehaviour
    {
        // Speed at which the random force is applied
        public float Speed;

        // Start is called before the first frame update
        void Start()
        {
            // Apply a random force to the object relative to its local space using the Rigidbody
            // Random.onUnitSphere generates a random point on the surface of a unit sphere
            // The force is multiplied by the Speed value to control its magnitude
            GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * Speed);
        }
    }
}
