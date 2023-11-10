using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC.Enemy;
using Player.Controller;

public class CheckEnemyInArea : MonoBehaviour
{
    public Button interactButton;
    private bool updateActive = true;

    void Start()
    {
        interactButton.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!updateActive)
            return;

        int killCount = Enemy.GetKillCount();

        if (killCount == 20)
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
