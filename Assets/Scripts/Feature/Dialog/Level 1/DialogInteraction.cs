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
        private bool updateEnabled = true; 

        private void Start()
        {
            // Cari objek pemain dalam skenario (misalnya, dengan tag "Player")
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (updateEnabled)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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
            Debug.Log("Interaction occurred.");

            if (interactButton.gameObject.activeSelf)
            {
                interactButton.onClick.Invoke();
                interactButton.gameObject.SetActive(false);
                updateEnabled = false;
            }
        }
    }
}
