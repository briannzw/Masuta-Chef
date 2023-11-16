using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioWinLose : MonoBehaviour
    {
        [Header("References")]
        public AudioSource audioSource;
        public AudioClip winClip;
        public AudioClip loseClip;
        public LevelManager levelManager;

        private void Start()
        {
            if (levelManager != null)
            {
                levelManager.OnLevelWin += PlayWinAudio;
                levelManager.OnLevelLose += PlayLoseAudio;
            }
            else
            {
                Debug.LogError("LevelManager reference is not set in AudioWinLose script.");
            }
        }

        private void PlayWinAudio()
        {
            if (audioSource != null && winClip != null)
            {
                audioSource.PlayOneShot(winClip);
            }
        }

        private void PlayLoseAudio()
        {
            if (audioSource != null && loseClip != null)
            {
                audioSource.PlayOneShot(loseClip);
            }
        }
    }
}
