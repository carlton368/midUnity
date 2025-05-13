using UnityEngine;
using System.Collections;

namespace AlignedGames
{
    public class FPSCounterBehaviour : MonoBehaviour
    {
        // Variable to calculate delta time (time between frames)
        float deltaTime = 0.0f;

        // Boolean to control whether FPS is shown on screen or not
        public bool Show_FPS;

        // Initialize the Show_FPS flag
        void Awake()
        {
            Show_FPS = false;  // By default, FPS display is off
        }

        // Update is called once per frame
        void Update()
        {
            // Smooth the deltaTime to prevent abrupt FPS changes
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

            // Check for the key press (L) to toggle FPS display
            if (Input.GetKeyDown(KeyCode.L))
            {
                // Toggle Show_FPS flag between true and false
                Show_FPS = !Show_FPS;
            }
        }

        // OnGUI is called for rendering and handling GUI events
        void OnGUI()
        {
            // Only show the FPS counter if Show_FPS is true
            if (Show_FPS)
            {
                int w = Screen.width;  // Screen width
                int h = Screen.height; // Screen height

                // Create a new GUIStyle to style the FPS text
                GUIStyle style = new GUIStyle
                {
                    alignment = TextAnchor.UpperLeft,  // Position the text at the upper left
                    fontSize = h * 2 / 100,            // Font size is 2% of the screen height
                    normal = { textColor = new Color(1.0f, 0.0f, 0.5f, 1.0f) } // Set the text color to red
                };

                // Create a Rect area where the FPS text will be shown (at the top-left of the screen)
                Rect rect = new Rect(0, 0, w, h * 2 / 100);

                // Calculate the milliseconds per frame and the FPS
                float msec = deltaTime * 1000.0f;  // Convert deltaTime to milliseconds
                float fps = 1.0f / deltaTime;     // Calculate FPS

                // Format the FPS and milliseconds into a string
                string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

                // Display the FPS and milliseconds on the screen
                GUI.Label(rect, text, style);
            }
        }
    }
}
