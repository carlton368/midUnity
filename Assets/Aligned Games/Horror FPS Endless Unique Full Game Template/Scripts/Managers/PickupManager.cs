using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Include this if using UnityEngine.UI for Text

namespace AlignedGames
{
    public class PickupManager : MonoBehaviour
    {
        // Audio clip for the pickup sound
        public AudioClip PickupSound;

        // UI Text object to display pickup messages
        public GameObject pickupUIText;

        // Duration for how long the message stays visible
        public float messageDuration = 2.0f;

        // Duration for how long the message fades out
        public float fadeDuration = 1.0f;

        // Distance the text floats upwards
        public float floatDistance = 50f;

        // Queue to store the pickup messages
        private Queue<string> pickupQueue = new Queue<string>();

        // Flag to check if a message is currently being displayed
        private bool isDisplayingMessage = false;

        // Store the original position of the text UI element
        private Vector3 originalTextPosition;

        // Start is called before the first frame update
        public void Start()
        {
            // Find the PickupText UI element and initialize it
            pickupUIText = GameObject.Find("PickupText");
            pickupUIText.GetComponent<Text>().text = "";

            // Save the original position of the text UI element for floating effect
            originalTextPosition = pickupUIText.GetComponent<RectTransform>().localPosition;
        }

        // Called when another collider enters the trigger collider
        public void OnTriggerEnter(Collider other)
        {
            // Check if the object that entered the trigger is a pickup
            if (other.tag == "Pickup")
            {
                // Play the pickup sound effect
                GetComponent<AudioSource>().PlayOneShot(PickupSound);

                string pickupMessage = ""; // Variable to hold the message

                // Check for health pack and apply its effects
                if (other.gameObject.GetComponent<PickupIdentifyBehaviour>().IsHealthPack)
                {
                    if (GetComponentInChildren<PlayerVitalsManager>().Health < GetComponentInChildren<PlayerVitalsManager>().MaxHealth)
                    {
                        GetComponentInChildren<PlayerVitalsManager>().Health += other.gameObject.GetComponent<PickupIdentifyBehaviour>().PickupAmount;
                        pickupMessage = "Picked up Health Pack";
                        Destroy(other.gameObject); // Destroy the health pack after pickup
                    }
                }

                // Check for shotgun ammo and apply its effects
                if (other.gameObject.GetComponent<PickupIdentifyBehaviour>().IsShotgunAmmunition)
                {
                    GetComponentInChildren<WeaponManager>().ShotgunMagsLeft += other.gameObject.GetComponent<PickupIdentifyBehaviour>().PickupAmount;
                    pickupMessage = "Picked up Shotgun Ammo";
                    Destroy(other.gameObject); // Destroy the ammo after pickup
                }

                // Check for machine gun ammo and apply its effects
                if (other.gameObject.GetComponent<PickupIdentifyBehaviour>().IsMachinegunAmmunition)
                {
                    GetComponentInChildren<WeaponManager>().MachineGunMagsLeft += other.gameObject.GetComponent<PickupIdentifyBehaviour>().PickupAmount;
                    pickupMessage = "Picked up Machine Gun Ammo";
                    Destroy(other.gameObject); // Destroy the ammo after pickup
                }

                // Check for pistol ammo and apply its effects
                if (other.gameObject.GetComponent<PickupIdentifyBehaviour>().IsPistolAmmunition)
                {
                    GetComponentInChildren<WeaponManager>().PistolMagsLeft += other.gameObject.GetComponent<PickupIdentifyBehaviour>().PickupAmount;
                    pickupMessage = "Picked up Pistol Ammo";
                    Destroy(other.gameObject); // Destroy the ammo after pickup
                }

                // Check for battery pack and apply its effects
                if (other.gameObject.GetComponent<PickupIdentifyBehaviour>().IsBatteryPack)
                {
                    GetComponentInChildren<FlashlightManager>().BatteryLeftAmount += other.gameObject.GetComponent<PickupIdentifyBehaviour>().PickupAmount;
                    pickupMessage = "Picked up Battery Pack";
                    Destroy(other.gameObject); // Destroy the battery pack after pickup
                }

                // Check for glowsticks pack and apply its effects
                if (other.gameObject.GetComponent<PickupIdentifyBehaviour>().IsGlowsticksPack)
                {
                    GetComponentInChildren<WeaponManager>().GlowsticksAmount += other.gameObject.GetComponent<PickupIdentifyBehaviour>().PickupAmount;
                    pickupMessage = "Picked up Glowsticks Pack";
                    Destroy(other.gameObject); // Destroy the glowsticks pack after pickup
                }

                // If a message was created, queue it for display
                if (!string.IsNullOrEmpty(pickupMessage))
                {
                    QueuePickupMessage(pickupMessage); // Queue the message to be displayed
                }
            }
        }

        // Adds a pickup message to the queue
        private void QueuePickupMessage(string message)
        {
            pickupQueue.Enqueue(message); // Add the message to the queue

            // If no message is currently displayed, start the display coroutine
            if (!isDisplayingMessage)
            {
                StartCoroutine(DisplayPickupMessages());
            }
        }

        // Coroutine to display the pickup messages
        private IEnumerator DisplayPickupMessages()
        {
            isDisplayingMessage = true; // Set flag indicating a message is being displayed

            // Loop until all messages in the queue are displayed
            while (pickupQueue.Count > 0)
            {
                // Get the next message in the queue
                string message = pickupQueue.Dequeue();
                pickupUIText.GetComponent<Text>().text = message;  // Set the UI text to the message
                pickupUIText.GetComponent<Text>().color = new Color(pickupUIText.GetComponent<Text>().color.r, pickupUIText.GetComponent<Text>().color.g, pickupUIText.GetComponent<Text>().color.b, 1f);

                // Reset text position to original position before starting to float
                pickupUIText.GetComponent<RectTransform>().localPosition = originalTextPosition;

                // Variables for floating and fading
                float elapsedTime = 0f;
                Vector3 startPos = originalTextPosition;
                Vector3 targetPos = originalTextPosition + new Vector3(0, floatDistance, 0);

                // While the message is visible
                while (elapsedTime < messageDuration + fadeDuration)
                {
                    // Animate floating upwards
                    float t = Mathf.Clamp01(elapsedTime / messageDuration);
                    pickupUIText.GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPos, targetPos, t);

                    // Start fading out after the messageDuration
                    if (elapsedTime > messageDuration)
                    {
                        float fadeTime = (elapsedTime - messageDuration) / fadeDuration;
                        float alpha = Mathf.Lerp(1f, 0f, fadeTime);
                        pickupUIText.GetComponent<Text>().color = new Color(pickupUIText.GetComponent<Text>().color.r, pickupUIText.GetComponent<Text>().color.g, pickupUIText.GetComponent<Text>().color.b, alpha);
                    }

                    elapsedTime += Time.deltaTime; // Increment the time
                    yield return null; // Wait for the next frame
                }

                // Clear the text after fade out
                pickupUIText.GetComponent<Text>().text = "";
            }

            isDisplayingMessage = false; // Set flag indicating no message is being displayed
        }
    }
}
