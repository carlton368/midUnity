using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace AlignedGames
{
    public class LogInManager : MonoBehaviour
    {
        // Reference to the InputField where the user enters their username
        public InputField UserName;
        // String to store the username entered by the user
        public string UserNameString;

        // Reference to the Start button that will be enabled/disabled
        public GameObject StartButton;

        // Start is called before the first frame update
        public void Start()
        {
            // Retrieve the saved username from PlayerPrefs (if available)
            UserNameString = PlayerPrefs.GetString("UserNameStringSaved");

            // Set the username text field to the saved username
            UserName.text = UserNameString;
        }

        // Update is called once per frame
        void Update()
        {
            // Update the UserNameString with the current text input from the user
            UserNameString = UserName.text;

            // Continuously save the username in PlayerPrefs
            PlayerPrefs.SetString("UserNameStringSaved", UserNameString);

            // Check if the username text is not empty
            if (UserName.text != "")
            {
                // Enable the Start button if the username is not empty
                StartButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                // Disable the Start button if the username is empty
                StartButton.GetComponent<Button>().interactable = false;
            }
        }
    }
}
