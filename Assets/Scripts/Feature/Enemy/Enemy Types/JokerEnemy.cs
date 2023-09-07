using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JokerEnemy : Enemy
{
    public override Transform TargetDestination => target;

    [SerializeField] private float wanderRadius = 10f;     // The radius within which the object can wander.
    [SerializeField] private float wanderTimer = 5f;       // Time between wandering direction changes.

    private Transform target;            // The position to move towards.
    private float timer;                 // Timer to control wandering direction changes.

    private new void Awake()
    {
        base.Awake();
        target = transform;
        StateMachine.Initialize(EnemyWanderState);
    }

    private new void Update()
    {
        timer -= Time.deltaTime;
        if (!Agent.isStopped)
        {
            if (timer <= 0f)
            {
                // Time to choose a new random destination.
                SetNewRandomDestination();
                timer = wanderTimer;
            }
        }
    }

    private void SetNewRandomDestination()
    {
        // Generate a random position within the wander radius.
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;

        // Project the position onto the NavMesh to find a valid destination.
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + randomDirection, out hit, wanderRadius, NavMesh.AllAreas);

        // Set the new target position.
        Agent.SetDestination(hit.position);
    }
}
