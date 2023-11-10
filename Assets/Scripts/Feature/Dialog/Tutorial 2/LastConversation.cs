using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC.Enemy;

public class LastConversation : MonoBehaviour
{

    private bool updateActive = true;
    public Button interactButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!updateActive)
            return;

        int killCount = Enemy.GetKillCount();

        if (killCount == 10)
        {
            ActivateDialogue();
            updateActive = false;
        }
    }

     public void ActivateDialogue()
    {
        interactButton.onClick.Invoke();
        interactButton.gameObject.SetActive(false);
    }
}
