using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject Player;
    PlayerHealth PlayerHealth;
    //EnemyHealth enemyHealth;
    bool PlayerInRange;
    float timer;


    void Awake ()
    {
        Player = GameObject.FindGameObjectWithTag ("Player");
        PlayerHealth = GetComponent <PlayerHealth> ();
        //enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == Player)
        {
            PlayerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == Player)
        {
            PlayerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && PlayerInRange/* && enemyHealth.currentHealth > 0*/)
        {
            Attack ();
        }

        if(PlayerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        timer = 0f;

        if(PlayerHealth.currentHealth > 0)
        {
            PlayerHealth.TakeDamage (attackDamage);
        }
    }
}
