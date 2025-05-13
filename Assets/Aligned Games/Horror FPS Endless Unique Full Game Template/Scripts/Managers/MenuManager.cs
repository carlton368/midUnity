using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Xml.Linq;

namespace AlignedGames
{
    public class MenuManager : MonoBehaviour
    {
        // Boolean to check if the game is in progress
        public bool InGame;

        // Reference to the Pause, Start, and End menus
        public GameObject PauseMenu;
        public GameObject StartMenu;
        public GameObject EndMenu;

        // Reference to the player object
        public GameObject Player;

        // Boolean to check if the game is paused
        public bool IsPaused;

        // URL strings to open different links
        public string URL1;
        public string URL2;

        // Start is called before the first frame update
        public void Start()
        {
            // If the game is in progress
            if (InGame)
            {
                // Set the appropriate menus visibility
                PauseMenu.SetActive(false);
                StartMenu.SetActive(true);
                EndMenu.SetActive(false);

                // Pause the game by setting time scale to 0
                Time.timeScale = 0;

                // Show the cursor and unlock it from the center of the screen
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // Disable player controls (camera and weapon management) during the start menu
                Player.GetComponentInChildren<PlayerCameraManager>().enabled = false;
                Player.GetComponentInChildren<WeaponManager>().enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the game is in progress
            if (InGame)
            {
                // Check for the Escape key press to pause or unpause the game
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    // If the game is not paused, pause it
                    if (!IsPaused)
                    {
                        IsPaused = true;

                        // Stop the game time and show the pause menu
                        Time.timeScale = 0;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;

                        // Disable player controls during pause
                        Player.GetComponentInChildren<PlayerCameraManager>().enabled = false;
                        Player.GetComponentInChildren<WeaponManager>().enabled = false;

                        // Activate the pause menu
                        PauseMenu.SetActive(true);
                    }
                    // If the game is already paused, resume it
                    else
                    {
                        IsPaused = false;

                        // Resume the game time and hide the pause menu
                        Time.timeScale = 1;
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;

                        // Enable player controls after unpausing
                        Player.GetComponentInChildren<PlayerCameraManager>().enabled = true;
                        Player.GetComponentInChildren<WeaponManager>().enabled = true;

                        // Deactivate the pause menu
                        PauseMenu.SetActive(false);
                    }
                }
            }
        }

        // Method to show the end menu when the game ends
        public void ShowEndMenu()
        {
            if (InGame)
            {
                InGame = false;

                // Show the end menu and pause the game
                EndMenu.SetActive(true);
                IsPaused = true;
                Time.timeScale = 0;

                // Unlock and show the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // Disable player controls during the end screen
                Player.GetComponentInChildren<PlayerCameraManager>().enabled = false;
                Player.GetComponentInChildren<WeaponManager>().enabled = false;
            }
        }

        // Method to restart the current level
        public void RestartLevel()
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        // Method to load the "TheTunnels" scene
        public void OpenTheTunnelsScene()
        {
            SceneManager.LoadScene("TheTunnels", LoadSceneMode.Single);
        }

        // Method to load the "MainMenu" scene
        public void OpenMainMenuScene()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        // Method to resume the game from the pause menu
        public void Resume()
        {
            IsPaused = false;

            // Resume the game time and hide the pause menu
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Enable player controls after resuming the game
            Player.GetComponentInChildren<PlayerCameraManager>().enabled = true;
            Player.GetComponentInChildren<WeaponManager>().enabled = true;

            // Deactivate the pause menu and start menu
            PauseMenu.SetActive(false);
            StartMenu.SetActive(false);
        }

        // Method to open the first URL in the browser
        public void OpenURL1()
        {
            Application.OpenURL(URL1);
        }

        // Method to open the second URL in the browser
        public void OpenURL2()
        {
            Application.OpenURL(URL2);
        }

        // Method to quit the application
        public void Quit()
        {
            Application.Quit();
        }
    }
}
