using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class DestroyTimerBehaviour : MonoBehaviour
    {

        public float Timer; // Timer value to track the countdown

        public bool UseRandomTimer;
        public bool IsObject;

        [SerializeField] private float minTime;
        [SerializeField] private float maxTime;


        void Start()

        {

            if (UseRandomTimer)

            {

                Timer = Random.Range(minTime, maxTime);
                Invoke("DestroyObject", Timer);

            }

        }

        private void DestroyObject()

        {

            if (IsObject)

            {

                GetComponent<ObjectHealthManager>().ApplyDamage(Mathf.Infinity);

            }

            else

            {

                Destroy(gameObject);
            }

        }

        void Update()
        {
            if (!UseRandomTimer)

            {
                Timer -= Time.deltaTime; // Decrease the timer based on the time elapsed since the last frame

                if (Timer <= 0) // Check if the timer has reached zero or below
                {
                    Destroy(this.gameObject); // Destroy the GameObject this script is attached to
                }
            }
        }

    }

}

