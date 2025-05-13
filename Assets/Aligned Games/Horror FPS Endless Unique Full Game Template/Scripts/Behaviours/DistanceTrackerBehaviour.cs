using UnityEngine;

namespace AlignedGames

{

    public class DistanceTrackerBehaviour : MonoBehaviour
    {
        public float tolerance = 0.1f; // Tolerance for deviation in direction.
        public float totalStraightDistance = 0f; // Total distance walked in a straight line.

        private Vector3 lastPosition;
        private Vector3 furthestPoint;
        private Vector3 initialForwardDirection; // Store the initial forward direction.

        void Start()
        {
            // Initialize last position, forward direction, and the furthest point.
            lastPosition = transform.position;
            furthestPoint = transform.position;
            initialForwardDirection = transform.forward.normalized; // Store the initial forward direction.
        }

        void Update()
        {
            // Get the current player position and movement direction.
            Vector3 currentPosition = transform.position;
            Vector3 movementDirection = (currentPosition - lastPosition).normalized;

            // Calculate how much the player has moved since the last frame.
            float distanceMoved = Vector3.Distance(currentPosition, lastPosition);

            // Check if the movement direction is close to the initial forward direction.
            if (Vector3.Dot(movementDirection, initialForwardDirection) > 1 - tolerance)
            {
                // If the player has moved beyond the furthest point, increase the straight distance.
                if (Vector3.Distance(currentPosition, furthestPoint) > Vector3.Distance(lastPosition, furthestPoint))
                {
                    totalStraightDistance += distanceMoved;
                    furthestPoint = currentPosition; // Update the furthest point.
                }
            }

            // Update the last position for the next frame.
            lastPosition = currentPosition;
        }
    }

}