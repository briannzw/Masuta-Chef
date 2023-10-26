using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueEditor
{
    public class Tutorial2Conversation : MonoBehaviour
    {
        public Button interactButton;

        // Start is called before the first frame update
        void Start()
        {
            interactButton.onClick.Invoke();
            interactButton.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}