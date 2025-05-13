using UnityEngine;

namespace AlignedGames

{

    public class InstanciationManager : MonoBehaviour
    {
        // Array to hold different sections
        public GameObject[] sections;

        // The spawn point where the section will be instantiated
        public Transform spawnPoint;

        // Function to instantiate a random section at the spawn point
        public void SpawnSection()
        {
            if (sections.Length == 0 || spawnPoint == null)
            {
                Debug.LogWarning("Sections array or spawn point is not set.");
                return;
            }

            // Pick a random section from the array
            int randomIndex = Random.Range(0, sections.Length);

            // Instantiate the selected section at the spawn point's position and rotation
            Instantiate(sections[randomIndex], spawnPoint.position, spawnPoint.rotation);
        }

    }

}