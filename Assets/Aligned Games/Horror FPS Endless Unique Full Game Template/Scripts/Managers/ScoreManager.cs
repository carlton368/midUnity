using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace AlignedGames
{
    // This script handles the scoring, distance tracking, and updating of UI elements related to the player's performance in a game.
    public class ScoreManager : MonoBehaviour
    {
        // Flags to indicate if the game is in the menu or level.
        public bool IsMenu;
        public bool IsLevel;

        // UI elements for displaying distance, score, and kills.
        public GameObject DistanceText;
        public GameObject ScoreText;
        public GameObject KillsText;

        // UI elements to display saved high scores.
        public GameObject SavedDistanceText;
        public GameObject SavedScoreText;
        public GameObject SavedKillsText;

        // UI elements for the end menu.
        public GameObject EndMenuDistanceText;
        public GameObject EndMenuScoreText;
        public GameObject EndMenuKillsText;

        // Variables to hold the player's distance, score, and kills.
        public float DistanceAmount;
        public float ScoreAmount;
        public float KillsAmount;

        // Multiplier for score based on kills.
        public float KillsAmountScoreMultiplier;

        // Variables to hold the high scores saved in PlayerPrefs.
        public float SavedHighScoreKillsAmount;
        public float SavedHighScoreDistanceAmount;
        public float SavedHighScoreAmount;

        // Reference to the player GameObject.
        public GameObject Player;

        // Start is called before the first frame update
        public void Start()
        {
            // Find the player object by its tag and load saved high scores from PlayerPrefs.
            Player = GameObject.FindWithTag("Player");

            SavedHighScoreDistanceAmount = PlayerPrefs.GetFloat("DistanceSaved", 0);
            SavedHighScoreAmount = PlayerPrefs.GetFloat("ScoreSaved", 0);
            SavedHighScoreKillsAmount = PlayerPrefs.GetFloat("KillsSaved", 0);
        }

        // Update is called once per frame
        public void Update()
        {
            // Check if the game is in the menu, and update UI elements accordingly.
            if (IsMenu)
            {
                // Find and display the saved high score values for kills, distance, and score.
                SavedKillsText = GameObject.Find("SavedKillsText");
                SavedKillsText.GetComponent<Text>().text = SavedHighScoreKillsAmount.ToString("0");

                SavedDistanceText = GameObject.Find("SavedDistanceText");
                SavedDistanceText.GetComponent<Text>().text = SavedHighScoreDistanceAmount.ToString("0");

                SavedScoreText = GameObject.Find("SavedScoreText");
                SavedScoreText.GetComponent<Text>().text = SavedHighScoreAmount.ToString("0");
            }

            // Check if the game is in a level, and update the score and distance.
            if (IsLevel)
            {
                // Calculate the total score (distance + kills * multiplier).
                ScoreAmount = DistanceAmount + KillsAmount * KillsAmountScoreMultiplier;

                // Update UI elements for kills, distance, and score.
                Player = GameObject.FindGameObjectWithTag("Player");

                // Update kills display if it's assigned.
                if (KillsText = GameObject.Find("KillsText"))
                {
                    KillsText.GetComponent<Text>().text = KillsAmount.ToString("0");
                }

                // Update distance display if it's assigned.
                if (DistanceText = GameObject.Find("DistanceText"))
                {
                    DistanceText.GetComponent<Text>().text = DistanceAmount.ToString("0");
                }

                // Update score display if it's assigned.
                if (ScoreText = GameObject.Find("ScoreText"))
                {
                    ScoreText.GetComponent<Text>().text = ScoreAmount.ToString("0");
                }

                // Save the highest score for kills, distance, and score if the current value exceeds the saved value.
                if (PlayerPrefs.GetFloat("KillsSaved") < KillsAmount)
                {
                    PlayerPrefs.SetFloat("KillsSaved", KillsAmount);
                }

                if (PlayerPrefs.GetFloat("DistanceSaved") < DistanceAmount)
                {
                    PlayerPrefs.SetFloat("DistanceSaved", DistanceAmount);
                }

                if (PlayerPrefs.GetFloat("ScoreSaved") < ScoreAmount)
                {
                    PlayerPrefs.SetFloat("ScoreSaved", ScoreAmount);
                }

                // Update the end menu UI with the final scores if the end menu is active.
                if (GetComponent<MenuManager>().EndMenu.activeSelf)
                {
                    if ((EndMenuKillsText) && (EndMenuDistanceText) && (EndMenuScoreText))
                    {
                        EndMenuKillsText = GameObject.Find("EndMenuKillsText");
                        EndMenuKillsText.GetComponent<Text>().text = KillsAmount.ToString("0");

                        EndMenuDistanceText = GameObject.Find("EndMenuDistanceText");
                        EndMenuDistanceText.GetComponent<Text>().text = DistanceAmount.ToString("0");

                        EndMenuScoreText = GameObject.Find("EndMenuScoreText");
                        EndMenuScoreText.GetComponent<Text>().text = ScoreAmount.ToString("0");

                        SavedKillsText = GameObject.Find("EndMenuSavedKillsText");
                        SavedKillsText.GetComponent<Text>().text = SavedHighScoreKillsAmount.ToString("0");

                        SavedDistanceText = GameObject.Find("EndMenuSavedDistanceText");
                        SavedDistanceText.GetComponent<Text>().text = SavedHighScoreDistanceAmount.ToString("0");

                        SavedScoreText = GameObject.Find("EndMenuSavedScoreText");
                        SavedScoreText.GetComponent<Text>().text = SavedHighScoreAmount.ToString("0");
                    }
                }

                // Update the distance amount based on the player's movement, if applicable.
                if (Player)
                {
                    if (Player.GetComponent<DistanceTrackerBehaviour>())
                    {
                        DistanceAmount = Player.GetComponent<DistanceTrackerBehaviour>().totalStraightDistance;
                    }
                }
            }
        }
    }
}
