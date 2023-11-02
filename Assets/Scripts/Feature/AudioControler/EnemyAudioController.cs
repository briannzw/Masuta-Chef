using UnityEngine;
using Character;

namespace Character
{
    public class EnemyAudioController : MonoBehaviour
    {
        public AudioSource deathSound;

        private void Start() 
        {
            Character character = GetComponent<Character>(); 
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
            Character character = GetComponent<Character>();
            if (character != null)
            {
                character.OnDie -= PlayDeathSound;
            }
        }

        public void PlayDeathSound()
        {
            if (deathSound != null)
            {
                deathSound.Play();
            }
        }
    }
}

