using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : Bullet
{
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Exploding Bullet");
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
