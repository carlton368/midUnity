using UnityEngine;
using UnityEngine.AI;

namespace AlignedGames
{
    // This class controls the enemy's AI behavior, including chasing the player, attacking, and handling melee or ranged combat
    public class EnemyAIManager : MonoBehaviour
    {
        // Reference to the NavMeshAgent used for moving the enemy
        public NavMeshAgent agent;

        // Reference to the player transform
        public Transform player;

        // LayerMasks to determine which objects are considered ground and player
        public LayerMask whatIsGround, whatIsPlayer;

        // Time between attacks and flag to check if an attack has already been made
        public float timeBetweenAttacks;
        bool alreadyAttacked;

        // Particle effect to spawn on attack
        public GameObject AttackParticle;

        // Detection ranges for sight and attack
        public float sightRange, attackRange;

        // Flags to check if the player is within sight or attack range
        public bool playerInSightRange, playerInAttackRange;

        // Reference to the spawn point of attacks
        public GameObject SpawnPoint;

        // Raycast hit variable for detecting collisions with the player during attacks
        RaycastHit Hit;

        // Timer variables for attack cooldown
        public float AttackTimer;
        public float TimeUntilAttack;

        // The damage dealt by the enemy
        public float Damage;

        // Prefab for ranged projectiles
        public GameObject Projectile;

        // Flags for determining whether the enemy is a melee or ranged attacker
        public bool MeleeEnemy;
        public bool RangedEnemy;

        // Initialize references
        private void Awake()
        {
            player = GameObject.Find("Player").transform;
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        public void Update()
        {
            // Check if the player is within sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            // If the player is in sight but not in attack range, chase the player
            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            else
            {
                // If the player is not in sight, play idle animation
                GetComponent<EnemyAnimationManager>().PlayIdleAnimation();
            }

            // If the player is in both sight and attack range, perform an attack
            if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            }
        }

        // Function to make the enemy chase the player
        public void ChasePlayer()
        {
            // Play running animation while chasing
            GetComponent<EnemyAnimationManager>().PlayRunAnimation();

            // Play chase sound while chasing
            GetComponent<EnemyAudioManager>().PlayEnemyChaseSound();

            // Set the agent's stopping distance to be the attack range (so it stops when in range)
            agent.stoppingDistance = attackRange;

            // Set the destination of the NavMeshAgent to the player's position
            agent.SetDestination(player.position);
        }

        // Function to handle attacking the player
        public void AttackPlayer()
        {
            // Stop the agent from moving while attacking
            agent.SetDestination(transform.position);

            // Make the enemy face the player
            transform.LookAt(player);

            // Ensure the attack is only performed once per attack cycle
            if (!alreadyAttacked)
            {
                // Play the attack animation
                GetComponent<EnemyAnimationManager>().PlayAttackAnimation();

                // Play attack sound while attacking
                GetComponent<EnemyAudioManager>().PlayEnemyAttackSound();

                // Increase the attack timer
                AttackTimer += Time.deltaTime;

                // If enough time has passed since the last attack, perform the attack
                if (AttackTimer >= TimeUntilAttack)
                {
                    // If the enemy is a melee attacker, perform a melee attack
                    if (MeleeEnemy)
                    {
                        // Perform a raycast from the spawn point to detect the player
                        if (Physics.Raycast(SpawnPoint.transform.position, SpawnPoint.transform.TransformDirection(Vector3.forward), out Hit, Mathf.Infinity))
                        {
                            // Debug the raycast for visualization (in the editor only)
                            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * Hit.distance, Color.yellow);

                            // Apply damage to the player if hit
                            Hit.transform.gameObject.GetComponent<PlayerVitalsManager>().SendMessage("ApplyDamage", Damage);

                            // Instantiate attack particle effect at the spawn point
                            Instantiate(AttackParticle, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
                        }
                    }

                    // If the enemy is a ranged attacker, perform a ranged attack
                    if (RangedEnemy)
                    {
                        // Instantiate the attack particle effect and projectile at the spawn point
                        Instantiate(AttackParticle, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
                        Instantiate(Projectile, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
                    }

                    // Set the attack flag to prevent further attacks until reset
                    alreadyAttacked = true;

                    // Reset the attack flag after a set time (cooldown)
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);

                    // Reset the attack timer
                    AttackTimer = 0;
                }
            }
        }

        // Function to reset the attack flag after cooldown
        public void ResetAttack()
        {
            alreadyAttacked = false;
        }

        // Function to draw Gizmos in the editor to visualize sight and attack ranges
        private void OnDrawGizmosSelected()
        {
            // Draw the attack range in red
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            // Draw the sight range in yellow
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }
}
