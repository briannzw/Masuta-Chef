using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossTurmeric : MonoBehaviour
{
    [SerializeField] float swampRadius = 5f;

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionLifetime = 2f;

    private float explosionInterval = 2f;  // Set your desired interval for explosions
    private int minExplosions = 6;         // Minimum number of explosions
    private int maxExplosions = 10;         // Maximum number of explosions

    private float nextExplosionTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Swamp();
    }

    void Swamp()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, swampRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {


                // Do DOT and apply slow debuff to Player
            }
        }

        if (Time.time > nextExplosionTime)
        {
            int numberOfExplosions = Random.Range(minExplosions, maxExplosions + 1);

            for (int i = 0; i < numberOfExplosions; i++)
            {
                Vector3 randomExplosionPosition = transform.position + Random.onUnitSphere * swampRadius;
                
                NavMeshHit hit;
                NavMesh.SamplePosition(transform.position + randomExplosionPosition, out hit, swampRadius, NavMesh.AllAreas);
                randomExplosionPosition.y = 0.5f;
                randomExplosionPosition = hit.position;
                GameObject explosion = Instantiate(explosionPrefab, randomExplosionPosition, Quaternion.identity);
                Destroy(explosion, explosionLifetime);
            }

            nextExplosionTime = Time.time + explosionInterval;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, swampRadius);
    }
}
