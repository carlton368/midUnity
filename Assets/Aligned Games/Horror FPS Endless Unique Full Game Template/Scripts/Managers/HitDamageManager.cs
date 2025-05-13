using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    // This script manages different hit damage zones (Limb, Body, Head) for an entity.
    public class HitDamageManager : MonoBehaviour
    {
        // Indicates if the hit is on a limb of the entity (e.g., arm or leg)
        public bool IsLimb;

        // Indicates if the hit is on the body of the entity (e.g., torso)
        public bool IsBody;

        // Indicates if the hit is on the head of the entity
        public bool IsHead;
    }
}
