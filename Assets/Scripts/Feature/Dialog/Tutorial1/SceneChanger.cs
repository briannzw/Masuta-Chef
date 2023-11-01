using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad; 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoad); 
    }
}
