using System.Collections;
using System.Collections.Generic;
using Character.Hit;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float TravelDistance;
    public float TravelSpeed;

    private Vector3 startPosition;
    [SerializeField] private LayerMask layerMask;
    
    private bool alreadyHit = false;
    public Weapon.Weapon weapon;
    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (alreadyHit) return;
        if (Vector3.Distance(startPosition, transform.position) >= Mathf.Abs(TravelDistance))
        {
            OnHit();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask.value) > 0)
        {
            // Play Some Effect
            alreadyHit = true;
            OnHit();
            Destroy(gameObject);
        }
    }

    protected virtual void OnHit()
    {
        // Do Something
    }
}
