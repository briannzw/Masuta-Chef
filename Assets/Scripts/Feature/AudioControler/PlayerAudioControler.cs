using UnityEngine;
using System;

namespace Player.Controller
{
    public class PlayerAudioController : MonoBehaviour
    {
        public AudioSource movementSound; 
        public AudioSource jumpSound; 
        public AudioSource deathSound; 

        private void Start()
        {
            PlayerMovementController.OnPlayerMove += PlayMovementSound;
        }

        private void OnDestroy()
        {
            PlayerMovementController.OnPlayerMove -= PlayMovementSound;
            
        }

        private void PlayMovementSound()
        {          
            if (movementSound != null)
            {
                movementSound.Play();
            }
        }

        public void PlayDeathSound()
        {           
            if (deathSound != null)
            {
                deathSound.Play();
            }
        }

        public void PlayJumpSound()
        {           
            if (jumpSound != null)
            {
                jumpSound.Play();
            }
        }
    }
}
