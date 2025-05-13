using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class EndColliderDetector : MonoBehaviour
    {
        public GameObject SceneManager; // Reference to the SceneManager GameObject

        public void Update()
        {
            // Continuously find the SceneManager GameObject in the scene
            SceneManager = GameObject.Find("SceneManager");
        }

        public void OnTriggerEnter(Collider other)
        {
            // Check if the object that triggered the collider is named "EndCollider"
            if (other.gameObject.name == "EndCollider")
            {
                SceneManager.GetComponent<MenuManager>().ShowEndMenu(); // Call the ShowEndMenu method on the MenuManager component
            }
        }
    }
}
