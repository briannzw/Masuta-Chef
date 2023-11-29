using System.Collections;
using System.Collections.Generic;
using Character.Hit;
using UnityEngine;
using Spawner;
using Weapon;

public class ExplodingBullet : Bullet
{
    [SerializeField] private LayerMask layerMask;
    private bool alreadyHit = false;

    [SerializeField] private float damageScaling;
    private TrailRenderer trail;

    [Header("Explosion Properties")]
    public GameObject explosionApplicator;
    // Start is called before the first frame update

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= Mathf.Abs(TravelDistance))
        {
            OnHit();
            GetComponent<SpawnObject>().Release();
        }
    }

    protected void OnHit()
    {
        GameObject gameObject = Instantiate(explosionApplicator, transform.position, transform.rotation);
        gameObject.GetComponent<HitController>().Initialize(gameObject.GetComponent<BulletHit>().Source, damageScaling);
    }

    private void OnEnable()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * TravelSpeed;
    }

    private void OnDisable()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (trail != null)
        {
            trail.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerMask.value) > 0)
        {
            // Play Some Effect
            alreadyHit = true;
            OnHit();
            GetComponent<SpawnObject>().Release();
        }
    }
}
