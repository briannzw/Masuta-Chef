using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldGrabber : MonoBehaviour
{
    [Header("From input field")]
    [SerializeField] private string inputText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrabInput(string input)
    {
        inputText = input;
    }

    public string GetInputText()
    { return inputText; }

}
