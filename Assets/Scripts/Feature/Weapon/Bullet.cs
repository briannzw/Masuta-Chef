using System.Collections;
using System.Collections.Generic;
using Character.Hit;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float TravelDistance;
    public float TravelSpeed;

    protected Vector3 startPosition;
    
    public Weapon.Weapon weapon;

    private void OnEnable()
    {
        startPosition = transform.position;
        GetComponent<Rigidbody>().velocity = transform.forward * TravelSpeed;
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= Mathf.Abs(TravelDistance))
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
