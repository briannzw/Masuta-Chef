using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueEditor
{
    public class OpeningConversation : MonoBehaviour
    {
        public Button interactButton1;

        // Start is called before the first frame update
        void Start()
        {
            interactButton1.onClick.Invoke();
            interactButton1.gameObject.SetActive(false);
        }
    }
}