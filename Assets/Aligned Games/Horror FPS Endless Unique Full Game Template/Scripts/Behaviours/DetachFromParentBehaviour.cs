using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class DetachFromParentBehaviour : MonoBehaviour
    {
        void Start()
        {
            transform.parent = null;  // Detaches this GameObject from its parent in the hierarchy
        }
    }
}
