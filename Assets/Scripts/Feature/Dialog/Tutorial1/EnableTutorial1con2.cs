using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableTutorial1con2 : MonoBehaviour
{
    public Button interactButton;
    public Image w;
    public Image a;
    public Image s;
    public Image d;
    public GameObject warning;
    public Image check1;
    public Image check2;

    private bool enableOnTriggerStay = false;
    private bool isClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        interactButton.gameObject.SetActive(true);
        warning.gameObject.SetActive(false);
        check1.gameObject.SetActive(false);
        check2.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!w.gameObject.activeSelf && !a.gameObject.activeSelf && !s.gameObject.activeSelf && !d.gameObject.activeSelf)
            {
                warning.gameObject.SetActive(false);
                interactButton.onClick.Invoke();
                interactButton.gameObject.SetActive(false);
            }
            else
            {
                warning.gameObject.SetActive(true);
            }
        }
    }

    public void EnableOnTriggerStay()
    {
        enableOnTriggerStay = true;
    }

    public void DestroyObject()
    {
        // Hancurkan objek ini
        Destroy(gameObject);
    }

    public void SetActive1()
    { check1.gameObject.SetActive(true); }
    public void SetActive2()
    { check2.gameObject.SetActive(true); }
}
