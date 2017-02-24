using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace CompleteProject
{
    public class PlayerHealth : MonoBehaviour
    {
        public int startingHealth = 100;                            // The amount of health the Player starts the game with.
        public int currentHealth;                                   // The current health the Player has.
        public AudioClip deathClip;                                 // The audio clip to play when the Player dies.
        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.


        Animator anim;                                              // Reference to the Animator component.
        AudioSource PlayerAudio;                                    // Reference to the AudioSource component.
        PlayerMovement PlayerMovement;                              // Reference to the Player's movement.
        PlayerShooting PlayerShooting;                              // Reference to the PlayerShooting script.
        bool isDead;                                                // Whether the Player is dead.
        bool damaged;                                               // True when the Player gets damaged.


        void Awake ()
        {
            // Setting up the references.
            anim = GetComponent <Animator> ();
            PlayerAudio = GetComponent <AudioSource> ();
            PlayerMovement = GetComponent <PlayerMovement> ();
            PlayerShooting = GetComponentInChildren <PlayerShooting> ();

            // Set the initial health of the 
            currentHealth = startingHealth;
        }

        public void TakeDamage (int amount)
        {
            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            // If the Player has lost all it's health and the death flag hasn't been set yet...
            if(currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }


        void Death ()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;

            // Turn off any remaining shooting effects.
            PlayerShooting.DisableEffects ();

            // Tell the animator that the Player is dead.
            anim.SetTrigger ("Die");

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
            PlayerAudio.clip = deathClip;
            PlayerAudio.Play ();

            // Turn off the movement and shooting scripts.
            PlayerMovement.enabled = false;
            PlayerShooting.enabled = false;
        }


        public void RestartLevel ()
        {
            // Reload the level that is currently loaded.
            SceneManager.LoadScene (0);
        }
    }
}