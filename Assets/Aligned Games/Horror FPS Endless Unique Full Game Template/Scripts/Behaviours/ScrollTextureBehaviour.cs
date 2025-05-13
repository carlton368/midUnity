using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    public class ScrollTextureBehaviour : MonoBehaviour
    {
        // Speed at which the texture will scroll
        public float scrollSpeed = 0.5F;

        // Reference to the Renderer component of the GameObject
        public Renderer rend;

        // Start is called before the first frame update
        void Start()
        {
            // Initialize the Renderer by getting the attached Renderer component
            rend = GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {
            // Calculate the texture offset based on the elapsed time and scroll speed
            float offset = Time.time * scrollSpeed;

            // Set the texture offset on the material to scroll the texture
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}
