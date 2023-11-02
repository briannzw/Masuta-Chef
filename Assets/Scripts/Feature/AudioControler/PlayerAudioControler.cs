using UnityEngine;
using Player.Controller;

namespace Character
{
    public class PlayerAudioController : MonoBehaviour
    {
        public AudioSource movementSound;
        public AudioSource jumpSound;
        public AudioSource deathSound;

        private Character character;
        private PlayerMovementController playerMovementController;

        private void Start()
        {
            character = GetComponent<Character>();
            playerMovementController = GetComponent<PlayerMovementController>();

            if (character != null)
            {
                character.OnDie += PlayDeathSound;
            }
            else
            {
                Debug.LogError("Komponen Character tidak ditemukan pada objek ini.");
            }
        }

        private void OnDestroy()
        {
            if (character != null)
            {
                character.OnDie -= PlayDeathSound;
            }
        }

        public void PlayMovementSound()
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
    



