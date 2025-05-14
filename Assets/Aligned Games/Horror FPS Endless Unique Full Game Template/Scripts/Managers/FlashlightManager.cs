using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace AlignedGames
{
    // This class manages the behavior of a flashlight in the game, including battery usage and toggling the light on/off
    public class FlashlightManager : MonoBehaviour
    {
        // The amount of battery remaining for the flashlight
        public float BatteryLeftAmount;
        // The rate at which the flashlight drains battery per second when on
        public float BatteryDrainAmount;
        // The amount of battery the player receives when picking up a battery pack
        public float BatteryPickupAmount;

        // The brightness level of the flashlight
        public float FlashlightBrightness;

        // A boolean to track whether the flashlight is turned on or off
        public bool IsLightOn;

        // Reference to the UI text object that displays the remaining battery amount
        // public GameObject FlashlightBatteryText;

        // A reference to a game object (possibly a trigger or indicator) related to the flashlight
        // public GameObject LightTrigger;

        // Start is called before the first frame update
        void Start()
        {
            // Initialize the flashlight to be off at the start of the game
            IsLightOn = false;
            
        }

        // Update is called once per frame
        void Update()
        {
            // Find the UI text object for battery display by name and update its value
            // FlashlightBatteryText = GameObject.Find("FlashlightBatteryText");
            // FlashlightBatteryText.GetComponent<Text>().text = BatteryLeftAmount.ToString("0");

            // Toggle the flashlight on/off when the player presses the "F" key
            if (Input.GetKeyDown(KeyCode.G))
            {
                IsLightOn = !IsLightOn;
            }

            // If the flashlight is on, drain the battery and update light intensity
            if (IsLightOn)
            {
                // Decrease the battery amount based on the drain rate and elapsed time
                BatteryLeftAmount -= Time.deltaTime * BatteryDrainAmount;
                // Update the light's intensity based on the battery level
                GetComponent<Light>().intensity = FlashlightBrightness * BatteryLeftAmount / 100;

                // Ensure the flashlight light component is enabled
                GetComponent<Light>().enabled = true;

                // Enable the LightTrigger object
                // LightTrigger.SetActive(true);
            }
            else
            {
                // If the flashlight is off, disable the light and set its intensity to 0
                GetComponent<Light>().enabled = false;
                GetComponent<Light>().intensity = 0;

                // Disable the LightTrigger object
                // LightTrigger.SetActive(false);
            }

            // Prevent the battery level from exceeding 100%
            if (BatteryLeftAmount >= 100)
            {
                BatteryLeftAmount = 100;
            }

            // Prevent the battery level from going below 0%
            if (BatteryLeftAmount <= 0)
            {
                BatteryLeftAmount = 0;
            }
        }

        // This function is called when the player picks up a battery pack
        public void PickupBatteryPack()
        {
            // Increase the battery amount by the pickup amount
            BatteryLeftAmount += BatteryPickupAmount;
        }
    }
}
