using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueEditor
{
    public class Tutorial1Conversation : MonoBehaviour
    {
        [SerializeField] private InputData inputData;

        public Button interactButton;
        public Image quest;
        public GameObject arrow;
        public Image w;
        public Image a;
        public Image s;
        public Image d;

        public MonoBehaviour[] componentsToPause;

        void Start()
        {
            interactButton.onClick.Invoke();
            interactButton.gameObject.SetActive(false);
            foreach (var component in componentsToPause)
            {
                if (component != null)
                {
                    component.enabled = false;
                }
            }

            inputData.savedInputText = "";
        }

        // Update is called once per frame
        void Update()
        {
            if (arrow.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    w.gameObject.SetActive(false);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    a.gameObject.SetActive(false);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    s.gameObject.SetActive(false);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    d.gameObject.SetActive(false);
                }
            }
        }

        public void SetActiveQuest(bool value)
        {
            quest.gameObject.SetActive(value);
        }
        public void SetActiveArrow(bool value)
        {
            arrow.gameObject.SetActive(value);
        }
    }
}