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
}
