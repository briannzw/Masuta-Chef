using UnityEngine;
using UnityEngine.UI;

namespace NPC.Dialogue
{
    using Interaction;

    public class DialogInteraction : MonoBehaviour, IInteractable
    {
        public Button interactButton;
        public float interactionDistance = 3.0f; // Jarak maksimum untuk interaksi

        private Transform player; // Transform pemain
        private bool updateEnabled = true; // Variabel untuk mengontrol Update()
        private bool isDialogActive = false;

        public MonoBehaviour[] componentsToPause; // Array komponen yang akan dijeda

        private void Start()
        {
            // Cari objek pemain dalam skenario (misalnya, dengan tag "Player")
            player = GameObject.FindGameObjectWithTag("Player").transform;

            // Disable the interaction button initially.
            interactButton.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (updateEnabled)
            {
                // Hitung jarak antara objek ini dan pemain
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // Periksa apakah objek ini berada dalam jarak interaksi
                if (distanceToPlayer <= interactionDistance)
                {
                    interactButton.gameObject.SetActive(true);
                }
                else
                {
                    interactButton.gameObject.SetActive(false);
                }
            }
        }

        public void Interact(GameObject other = null)
        {
            // Implement your interaction logic here.
            Debug.Log("Interaction occurred.");

            // Jalankan metode onclick() pada tombol jika tombol aktif
            if (interactButton.gameObject.activeSelf)
            {
                // Jeda semua komponen yang perlu dijeda
                foreach (var component in componentsToPause)
                {
                    if (component != null)
                    {
                        component.enabled = false;
                    }
                }

                // Set isDialogActive menjadi true
                isDialogActive = true;

                // Jalankan metode onclick() pada tombol
                interactButton.onClick.Invoke();
                interactButton.gameObject.SetActive(false);
                updateEnabled = false;
            }
        }

        // Metode untuk melanjutkan permainan setelah dialog selesai
        public void ResumeGame()
        {
            // Lanjutkan semua komponen yang dijeda
            foreach (var component in componentsToPause)
            {
                if (component != null)
                {
                    component.enabled = true;
                }
            }

            // Set isDialogActive menjadi false
            isDialogActive = false;
        }
    }
}
