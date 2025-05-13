using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class PickupIdentifyBehaviour : MonoBehaviour
    {
        // Flags to identify the type of pickup
        public bool IsHealthPack;
        public bool IsArmourPack;
        public bool IsBatteryPack;
        public bool IsGlowsticksPack;

        // Flags to identify the type of ammunition
        public bool IsShotgunAmmunition;
        public bool IsMachinegunAmmunition;
        public bool IsPistolAmmunition;

        // The amount of resource the pickup provides
        public float PickupAmount;
    }
}
