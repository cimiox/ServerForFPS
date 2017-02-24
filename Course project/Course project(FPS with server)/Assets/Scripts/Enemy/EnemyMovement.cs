using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform Player;
    //PlayerHealth PlayerHealth;
    //EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;


    void Awake ()
    {
        Player = GameObject.FindGameObjectWithTag ("Player").transform;
        //PlayerHealth = GetComponent <PlayerHealth> ();
        //enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }


    void Update ()
    {
        //if(enemyHealth.currentHealth > 0 && PlayerHealth.currentHealth > 0)
        //{
            nav.SetDestination (transform.position);
        //}
        //else
        //{
        //    nav.enabled = false;
        //}
    }
}
