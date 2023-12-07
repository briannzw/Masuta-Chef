using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crate.Area;

namespace DialogueEditor
{
    public class Tutorial2Conversation3 : MonoBehaviour
    {
        public Button interactButton2;
        public Button interactButton3;
        public Button interactButton4;
        public Button interactButton5;
        public Button interactButton6;
        public Button interactButton7;
        public Button interactButton8;
        private CrateArea crateArea;
        private bool updateActive = true;
        private bool interactButton4Activated = false;
        private bool interactButton5Activated = false;
        private bool interactButton6Activated = false;
        private bool interactButton8Activated = false;

        private InputFieldGrabber inputFieldGrabber;

        // Start is called before the first frame update
        void Start()
        {
            crateArea = FindObjectOfType<CrateArea>();
            inputFieldGrabber = FindObjectOfType<InputFieldGrabber>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!updateActive)
                return;

            EnableCon();
        }

        public void EnableCon()
        {
            int crateCount = crateArea.GetCurrentCrateCount();
            bool isCombined = crateArea.GetInteractStatus();
            bool isSameColor = crateArea.GetCurrentCrateColor();
            if (crateCount == 1)
            {
                if (interactButton2.gameObject.activeSelf)
                {
                    interactButton7.onClick.Invoke();
                    interactButton2.onClick.Invoke();
                    interactButton2.gameObject.SetActive(false);
                    updateActive = false;

                }
            }
            else if (crateCount == 4 && !interactButton6Activated)
            {
                interactButton7.gameObject.SetActive(true);
                interactButton7.onClick.Invoke();

                interactButton6.gameObject.SetActive(true);
                interactButton6Activated = true;
            }
            else if (crateCount == 3 && !interactButton5Activated)
            {
                interactButton7.gameObject.SetActive(true);
                interactButton7.onClick.Invoke();
                
                interactButton5.gameObject.SetActive(true);
                interactButton5Activated = true;
            }
            else if (crateCount == 2 )
            {
                if (isSameColor && !interactButton4Activated)
                {
                    interactButton7.gameObject.SetActive(true);
                    interactButton7.onClick.Invoke();
                
                    interactButton4.gameObject.SetActive(true);
                    interactButton4Activated = true;
                    interactButton8Activated = true;
                }
                else if (!isSameColor && !interactButton8Activated)
                {
                    interactButton8.onClick.Invoke();
                    interactButton8Activated = true;
                }
            }
            else if (crateCount == 0 && isCombined)
            {
                if (interactButton3.gameObject.activeSelf)
                {
                    interactButton3.onClick.Invoke();
                    interactButton3.gameObject.SetActive(false);
                    updateActive = false;
                }
                else if (interactButton4.gameObject.activeSelf)
                {
                    interactButton4.onClick.Invoke();
                    interactButton4.gameObject.SetActive(false);
                    updateActive = false;
                }
                else if (interactButton5.gameObject.activeSelf)
                {
                    interactButton5.onClick.Invoke();
                    interactButton5.gameObject.SetActive(false);
                    updateActive = false;
                }
                else if (interactButton6.gameObject.activeSelf)
                {
                    interactButton6.onClick.Invoke();
                    interactButton6.gameObject.SetActive(false);
                    updateActive = false;
                }
            }
            interactButton7.gameObject.SetActive(false);
        }

        public void SetInputFieldNull()
        {
            inputFieldGrabber = null;
        }

        public void ResumeUpdate()
        { updateActive = true; }
    }
}