using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JokerEnemy : Enemy
{
    public override Transform TargetDestination => target;

    [SerializeField] private float wanderRadius = 10f;     // The radius within which the object can wander.
    [SerializeField] private float wanderTimer = 5f;       // Time between wandering direction changes.
    [SerializeField] private float detectionRadius = 5f;    // Detect nearby crate radius.

    private Transform target;            // The position to move towards.
    private float timer;                 // Timer to control wandering direction changes.
    private GameObject nearestCrate;
    public JokerChaseCrateState JokerChaseCrateState { get; set; }

    private new void Awake()
    {
        base.Awake();
        target = transform;
        StateMachine.Initialize(EnemyWanderState);
        JokerChaseCrateState = new JokerChaseCrateState(this, StateMachine);
    }

    private new void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
        Agent.speed = MoveSpeed;
        CheckForCrate();

        if (StateMachine.CurrentEnemyState == EnemyWanderState && !Agent.isStopped)
        {
            timer -= Time.deltaTime;
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

    private void CheckForCrate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        float nearestDistance = Mathf.Infinity;

        // Track whether a crate is currently detected
        bool isCrateDetected = false;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Crate"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // Check if this is the nearest crate
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestCrate = collider.gameObject;
                    isCrateDetected = true;  // Set the flag to true since a crate is detected
                }
            }
        }

        // If a crate is detected, chase it; otherwise, wander
        if (isCrateDetected)
        {
            StateMachine.ChangeState(JokerChaseCrateState);
            Agent.isStopped = false;
            Agent.SetDestination(nearestCrate.transform.position);
        }
        else
        {
            StateMachine.ChangeState(EnemyWanderState);
        }
    }
}
