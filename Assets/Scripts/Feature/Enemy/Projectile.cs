using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float AutoDestroyTime = 1f;

    [SerializeField] private float Force = 100;

    private WaitForSeconds Wait;
    private Rigidbody Rigidbody;
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(DelayDisable());
    }

    private IEnumerator DelayDisable()
    {
        if (Wait == null)
        {
            Wait = new WaitForSeconds(AutoDestroyTime);
        }

        yield return null;

        Rigidbody.AddForce(transform.forward * Force);

        yield return Wait;
        gameObject.SetActive(false);
    }

    private void Update()
    {

        // Calculate the direction to the player
        Vector3 directionToPlayer = EnemySpawner.Instance.PlayerPosition.position - transform.position;

        // Set the rotation to face the player instantly
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }

    private void OnDisable()
    {
        Rigidbody.angularVelocity = Vector3.zero;
        Rigidbody.velocity = Vector3.zero;
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // you'd want to apply damage or something here as well.
        Destroy(gameObject);
    }
}
