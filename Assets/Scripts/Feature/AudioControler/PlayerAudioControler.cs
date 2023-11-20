using UnityEngine;
using System;

namespace Player.Controller
{
    public class PlayerAudioController : MonoBehaviour
    {
        public AudioSource movementSound; 
        public AudioSource jumpSound; 

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
            // Memainkan suara pergerakan
            if (movementSound != null)
            {
                movementSound.Play();
            }
        }

        public void PlayJumpSound()
        {
            // Memainkan suara lompat
            if (jumpSound != null)
            {
                jumpSound.Play();
            }
        }
    }
}

