using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform Player;               // Reference to the Player's position.
        PlayerHealth PlayerHealth;      // Reference to the Player's health.
        EnemyHealth enemyHealth;        // Reference to this enemy's health.
        UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.


        void Awake ()
        {
            // Set up the references.
            Player = GameObject.FindGameObjectWithTag ("Player").transform;
            PlayerHealth = GetComponent <PlayerHealth> ();
            enemyHealth = GetComponent <EnemyHealth> ();
            nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
        }


        void Update ()
        {
            // If the enemy and the Player have health left...
            if(enemyHealth.currentHealth > 0 && PlayerHealth.currentHealth > 0)
            {
                // ... set the destination of the nav mesh agent to the 
                nav.SetDestination (transform.position);
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                nav.enabled = false;
            }
        }
    }
}