using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlignedGames
{
    // This class manages the enemy's animations, including playing different animations based on the enemy's actions
    public class EnemyAnimationManager : MonoBehaviour
    {
        // The name of the flinch animation
        public string FlinchAnimation;
        // The name of the attack animation
        public string AttackAnimation;
        // The name of the running animation
        public string RunAnimation;
        // The name of the idle animation
        public string IdleAnimation;

        // This function plays the flinch animation on the enemy
        public void PlayFlinchAnimation()
        {
            // Play the flinch animation using the specified animation name
            GetComponent<Animation>().Play(FlinchAnimation);
        }

        // This function plays the attack animation with a crossfade effect
        public void PlayAttackAnimation()
        {
            // Crossfade to the attack animation smoothly
            GetComponent<Animation>().CrossFade(AttackAnimation);
        }

        // This function plays the running animation with a crossfade effect
        public void PlayRunAnimation()
        {
            // Crossfade to the running animation smoothly
            GetComponent<Animation>().CrossFade(RunAnimation);
        }

        // This function plays the idle animation with a crossfade effect
        public void PlayIdleAnimation()
        {
            // Crossfade to the idle animation smoothly
            GetComponent<Animation>().CrossFade(IdleAnimation);
        }
    }
}
