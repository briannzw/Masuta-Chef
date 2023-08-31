using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{

    private Transform destination;
    [SerializeField] private float distanceToDestination = 5f;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        destination = EnemySpawner.Instance.PlayerPosition;
    }

    private void Update()
    {
        agent.destination = destination.position;
        if (agent.remainingDistance < distanceToDestination)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }
}
