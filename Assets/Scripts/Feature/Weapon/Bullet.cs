using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float TravelDistance;
    public float TravelSpeed;
    public LayerMask HitLayer;

    private Vector3 startPosition;

    [Header("Exploding Bullet Parameter")]
    public bool isExploding = false;
    [SerializeField] [ConditionalField(nameof(isExploding))]
    private GameObject aoeEffectApplicator;

    private void Start()
    {
        startPosition = transform.position;
        this.GetComponent<Rigidbody>().velocity = transform.forward * TravelSpeed;
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= Mathf.Abs(TravelDistance))
        {
            Hit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & HitLayer.value) > 0)
        {
            Hit();
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if ((HitLayer | (1 << collision.gameObject.layer)) > 0)
    //    {
    //        Hit();
    //    }
    //}

    private void Hit()
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        if (!isExploding || aoeEffectApplicator == null) return;
        
        Instantiate(aoeEffectApplicator,transform.position, transform.rotation);
    }
}
