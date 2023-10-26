using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC.Enemy;
using Player.Controller;

public class EnableTutorial1con3 : MonoBehaviour
{
    public Button interactButton;
    private bool updateActive = true;
    public Image check2;
    public Image check3;

    public PlayerWeaponController playerWeaponController;

    void Start()
    {
        check3.gameObject.SetActive(false);
        interactButton.gameObject.SetActive(true);
    }

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

    public void OnTriggerStay(Collider other)
    {
        if (playerWeaponController.WeaponTransform.gameObject.activeSelf && other.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetActive2();
            }
        }
    }

    public void SetActive2()
    { check2.gameObject.SetActive(true); }
    public void SetActive3()
    { check3.gameObject.SetActive(true); }

    public void ActivateDialogue()
    {
        interactButton.onClick.Invoke();
        interactButton.gameObject.SetActive(false);
    }
}
