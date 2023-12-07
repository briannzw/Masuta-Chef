using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC.Enemy;
using Level;

public class LastConversation : MonoBehaviour
{
    public LevelManager levelManager;
    public Button interactButton;

    private bool updateActive = true;

    void Update()
    {
        if (!updateActive)
            return;

        int killCount = levelManager.GetCurrentEnemyCount("Enemy");

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
