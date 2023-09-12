using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JokerEnemy : Enemy
{
    
    public override float WanderRadius => wanderRadius;
    public override float WanderTimer => wanderTimer;
    public override float CrateDetectionRadius => detectionRadius;
    [Header("Joker Properties")]
    [SerializeField] private float wanderRadius = 10f;     // The radius within which the object can wander.
    [SerializeField] private float wanderTimer = 5f;       // Time between wandering direction changes.
    [SerializeField] private float detectionRadius = 5f;    // Detect nearby crate radius.

    private Transform target;            // The position to move towards.
    private float timer;                 // Timer to control wandering direction changes.
    private GameObject nearestCrate;

    private new void Awake()
    {
        base.Awake();
        target = transform;
        StateMachine.Initialize(EnemyWanderState);
    }

    private new void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
        Agent.speed = MoveSpeed;
        CheckForCrate();
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

    private void OnDrawGizmos()
    {
        // Set the color of the Gizmo sphere
        Gizmos.color = Color.yellow;

        // Draw a wire sphere at the JokerEnemy's position with the specified detectionRadius
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
