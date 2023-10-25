using System.Collections;
using System.Collections.Generic;
using Character.Hit;
using UnityEngine;
using Weapon;

public class ExplodingBullet : Bullet
{
    [SerializeField] private LayerMask layerMask;
    private bool alreadyHit = false;

    [Header("Explosion Properties")]
    public GameObject explosionApplicator;
    // Start is called before the first frame update
    protected void OnHit()
    {
        GameObject gameObject = Instantiate(explosionApplicator, transform.position, transform.rotation);
        gameObject.GetComponentInChildren<AOEController>().Initialize(weapon);
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

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= Mathf.Abs(TravelDistance))
        {
            OnHit();
            Destroy(gameObject);
        }
    }
}
