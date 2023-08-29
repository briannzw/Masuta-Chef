using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

    private Transform destination;
    [SerializeField] private float distanceToDestination = 5f;
    private NavMeshAgent agent;
    private float timerSinceSpawn = 1f;

    void Start()
    {
        destination = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(timerSinceSpawn > 0)
        {
            timerSinceSpawn -= Time.deltaTime;
            agent.ResetPath();
        }


        if (Vector3.Distance(transform.position, destination.position) < distanceToDestination)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void FixedUpdate()
    {
        agent.destination = destination.position;

    }
}
