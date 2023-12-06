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
    public Image check4;
    public GameObject waveManager;

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

        if (killCount == 5)
        {
            ActivateDialogue();
            waveManager.gameObject.SetActive(false);
            updateActive = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (playerWeaponController.WeaponTransform.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetActive2();
            }
            
            if(Input.GetKeyDown(KeyCode.Q))
            {
                SetActive3();
            }
        }
    }

    public void SetActive2()
    { check2.gameObject.SetActive(true); }
    public void SetActive3()
    { check3.gameObject.SetActive(true); }
    public void SetActive4()
    { check4.gameObject.SetActive(true); }

    public void ActivateDialogue()
    {
        SetActive4();
        interactButton.onClick.Invoke();
        interactButton.gameObject.SetActive(false);
    }
}
