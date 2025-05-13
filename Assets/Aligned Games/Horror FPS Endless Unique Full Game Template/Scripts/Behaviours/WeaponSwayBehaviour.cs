using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class WeaponSwayBehaviour : MonoBehaviour
    {
        // Stores the current and new rotation values for weapon sway
        private Quaternion CurrentPos;
        private Quaternion NewPos;
        private Quaternion NewPosJoystick;

        // Speed multiplier for the sway effect
        public float SwaySpeed;

        void Update()
        {
            // Ensure sway calculations only happen when time is progressing (prevents issues like during pause)
            if (Time.deltaTime != 0)
            {
                // Check if the weapon is not being aimed
                if (!GetComponent<WeaponAnimatorBehaviour>().isAiming)
                {
                    // Store the current weapon rotation but keep the Z-axis rotation at 0 to prevent roll
                    CurrentPos = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 0));

                    // Get mouse input for sway
                    float MouseX = Input.GetAxis("Mouse X") * SwaySpeed;
                    float MouseY = Input.GetAxis("Mouse Y") * SwaySpeed * 2;

                    // Get controller right stick input for sway
                    float RightStickXAxis = Input.GetAxis("Right Stick X Axis");
                    float RightStickYAxis = Input.GetAxis("Right Stick Y Axis");

                    // Adjust mouse sway values
                    MouseX -= 1f;
                    MouseX = Mathf.Clamp(MouseX, -15, 15);

                    // Adjust right stick sway values
                    RightStickXAxis -= 1f;
                    RightStickXAxis = Mathf.Clamp(RightStickXAxis, -15, 15);

                    // Calculate new sway position based on mouse movement
                    NewPos = Quaternion.Lerp(CurrentPos, Quaternion.Euler(MouseY, MouseX, 0), 0.2f);

                    // Calculate new sway position based on joystick movement
                    NewPosJoystick = Quaternion.Lerp(CurrentPos, Quaternion.Euler(RightStickYAxis, RightStickXAxis, 0), 0.2f);

                    // Check if there is any input from the right stick and apply joystick sway if true
                    if ((Input.GetAxis("Right Stick Y Axis") > 0) || (Input.GetAxis("Right Stick Y Axis") < 0) ||
                        (Input.GetAxis("Right Stick X Axis") < 0) || (Input.GetAxis("Right Stick X Axis") > 0))
                    {
                        transform.localRotation = Quaternion.Euler(NewPosJoystick.eulerAngles);
                    }
                    else
                    {
                        // Otherwise, apply mouse sway
                        transform.localRotation = Quaternion.Euler(NewPos.eulerAngles);
                    }
                }
            }
        }
    }
}
