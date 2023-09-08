using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float aliveTime = 3f;

    private void Start() 
    {
        Destroy(this.gameObject,aliveTime);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Hit Object" + other.gameObject.name);
        // Do Something
    }
}
