using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlignedGames
{
    public class TriggerBehaviour : MonoBehaviour
    {
        // Flags to determine different types of triggers
        public bool IsDoorSwitch; // Whether the trigger is for switching the door state
        public bool IsBehindDoorTrigger; // Whether the trigger is for the door closing
        public bool IsBehindSectionDestroyTrigger; // Whether the trigger destroys a section behind it

        // Reference to spawn point for the section
        public GameObject SectionSpawnPoint;

        // Reference to the door that will be opened/closed by this trigger
        public GameObject DoorToSwitch;

        // Reference to the SceneManager to handle scene-related tasks
        public GameObject SceneManager;

        // Flag to track if the trigger has already been activated
        public bool HasTriggered;

        // Start is called before the first frame update
        public void Start()
        {
            // Find and assign the SceneManager object
            SceneManager = GameObject.Find("SceneManager");
        }

        // Trigger event when another collider enters the trigger area
        public void OnTriggerEnter(Collider Other)
        {
            // Check if the colliding object is the player
            if (Other.gameObject.tag == "Player")
            {
                // If it's a behind-door trigger, play door close animation and destroy the trigger object
                if (IsBehindDoorTrigger)
                {
                    DoorToSwitch.GetComponent<Animation>().Play("door_1_close_1");
                    Destroy(this.transform.gameObject);
                }

                // If it's a section destroy trigger, destroy the parent object of the trigger
                if (IsBehindSectionDestroyTrigger)
                {
                    Destroy(this.transform.parent.gameObject);
                }

                // Mark the trigger as having been activated
                HasTriggered = true;
            }
        }

        // Trigger event when another collider stays within the trigger area
        public void OnTriggerStay(Collider Other)
        {
            // Check if the colliding object is the player
            if (Other.gameObject.tag == "Player")
            {
                // If it's a behind-door trigger, play door close animation and destroy the trigger object
                if (IsBehindDoorTrigger)
                {
                    DoorToSwitch.GetComponent<Animation>().Play("door_1_close_1");
                    Destroy(this.transform.gameObject);
                }

                // If it's a section destroy trigger, destroy the parent object of the trigger
                if (IsBehindSectionDestroyTrigger)
                {
                    Destroy(this.transform.parent.gameObject);
                }

                // Mark the trigger as having been activated
                HasTriggered = true;
            }
        }

        // Method to switch the door state (open it) and spawn a new section
        public void SwitchDoor()
        {
            // If the trigger hasn't been activated yet
            if (!HasTriggered)
            {
                // Play the switch flip animation
                GetComponent<Animation>().Play("flip_switch_1");
                ChangeSwitchEmissionColor(); // Change the emission color to green for the switch

                // Play the door opening animation
                DoorToSwitch.GetComponent<Animation>().Play("door_1_open_1");

                // Set the spawn point for the section and spawn it
                SceneManager.GetComponent<InstanciationManager>().spawnPoint = SectionSpawnPoint.transform;
                SceneManager.GetComponent<InstanciationManager>().SpawnSection();

                // Mark the trigger as having been activated
                HasTriggered = true;
            }
        }

        // Method to change the emission color of the switch to green
        void ChangeSwitchEmissionColor()
        {
            // Get the renderer of the trigger's child object
            Renderer renderer = GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Get the material instance of the renderer
                Material materialInstance = renderer.material;

                // Enable emission for the material
                materialInstance.EnableKeyword("_EMISSION");

                // Set the emission color to green
                materialInstance.SetColor("_EmissionColor", Color.green);
            }
        }
    }
}
