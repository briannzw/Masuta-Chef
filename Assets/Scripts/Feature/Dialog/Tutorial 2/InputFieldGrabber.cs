using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputFieldGrabber : MonoBehaviour
{
    [Header("From input field")]
    [SerializeField] private string inputText;

    [Header("GameObject")]
    [SerializeField] private GameObject warning;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button submitButton;

    // Start is called before the first frame update
    void Start()
    {
        warning.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GrabInput(string input)
    {
        inputText = input;
    }

    public void CheckInputText()
    {
        if (string.IsNullOrEmpty(inputText) && submitButton.gameObject.activeSelf)
        {
            warning.SetActive(true);
        }
        else
        {
            interactButton.onClick.Invoke();
            warning.SetActive(false);
        }
    }

    public void OnSubmitButtonClick()
    {
        if (Input.GetKey(KeyCode.Return) && submitButton.gameObject.activeSelf && !string.IsNullOrEmpty(inputText))
        {
            submitButton.onClick.Invoke();
        }
    }

    public string GetInputText()
    { return inputText; }

}
