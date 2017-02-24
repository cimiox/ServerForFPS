using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int attackDamage = 10;               // The amount of health taken away per attack.


        Animator anim;                              // Reference to the animator component.
        GameObject Player;                          // Reference to the Player GameObject.
        PlayerHealth PlayerHealth;                  // Reference to the Player's health.
        EnemyHealth enemyHealth;                    // Reference to this enemy's health.
        bool PlayerInRange;                         // Whether Player is within the trigger collider and can be attacked.
        float timer;                                // Timer for counting up to the next attack.


        void Awake ()
        {
            // Setting up the references.
            Player = GameObject.FindGameObjectWithTag ("Player");
            PlayerHealth = GetComponent <PlayerHealth> ();
            enemyHealth = GetComponent<EnemyHealth>();
            anim = GetComponent <Animator> ();
        }


        void OnTriggerEnter (Collider other)
        {
            // If the entering collider is the ..
            if(other.gameObject == Player)
            {
                // ... the Player is in range.
                PlayerInRange = true;
            }
        }


        void OnTriggerExit (Collider other)
        {
            // If the exiting collider is the ..
            if(other.gameObject == Player)
            {
                // ... the Player is no longer in range.
                PlayerInRange = false;
            }
        }


        void Update ()
        {
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;

            // If the timer exceeds the time between attacks, the Player is in range and this enemy is alive...
            if(timer >= timeBetweenAttacks && PlayerInRange && enemyHealth.currentHealth > 0)
            {
                // ... attack.
                Attack ();
            }

            // If the Player has zero or less health...
            if(PlayerHealth.currentHealth <= 0)
            {
                // ... tell the animator the Player is dead.
                anim.SetTrigger ("PlayerDead");
            }
        }


        void Attack ()
        {
            // Reset the timer.
            timer = 0f;

            // If the Player has health to lose...
            if(PlayerHealth.currentHealth > 0)
            {
                // ... damage the 
                PlayerHealth.TakeDamage (attackDamage);
            }
        }
    }
}