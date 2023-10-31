using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float TravelDistance;
    public float TravelSpeed;

    private Vector3 startPosition;
    
    private void Start()
    {
        startPosition = transform.position;
        this.GetComponent<Rigidbody>().velocity = transform.forward * TravelSpeed;
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= Mathf.Abs(TravelDistance))
        {
            Destroy(gameObject);
        }
    }
}
