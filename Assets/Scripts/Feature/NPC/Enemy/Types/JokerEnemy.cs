using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JokerEnemy : Enemy, IDetectionNPC, IWanderNPC
{
    [field: SerializeField] public float DetectionRadius { get; set; }
    public string TargetTag { get; set; }
    [field: SerializeField] public float WanderRadius { get; set; }
    [field: SerializeField] public float WanderInterval { get; set; }
    [SerializeField] float wanderTimer = 0;
    private bool isPickingUpCrate = false;
    public LayerMask TargetMask;
    [Range(0, 360)] public float PickupAngle = 125;
    private Pickup.IPickable nearestPickable;
    [SerializeField] private Transform pickupPos;
    private bool hasCrate => nearestPickable != null;
    [SerializeField] private float safeDistance = 10f;

    private new void Update()
    {
        StateMachine.CurrentState.FrameUpdate();
        if (Agent.remainingDistance <= StopDistance)
        {
            Agent.isStopped = true;
        }
        else
        {
            Agent.isStopped = false;
        }

        DetectTarget();
        if (!isPickingUpCrate && !hasCrate)
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f)
            {
                // Time to choose a new random destination.
                Wander();
                wanderTimer = WanderInterval;
            }
        }

        if (hasCrate)
        {
            RunAwayFromPlayer();
        }
    }

    public void DetectTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, DetectionRadius);
        float closestDistance = Mathf.Infinity;
        Transform closestCrate = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Crate"))
            {
                float distanceToCrate = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToCrate < closestDistance)
                {
                    closestDistance = distanceToCrate;
                    closestCrate = collider.transform;
                }
            }
        }

        if (closestCrate != null)
        {
            isPickingUpCrate = true;
            TargetPosition = closestCrate.position;
            if (Agent.remainingDistance <= StopDistance)
            {
                PickUpCrate();
            }
        }
        else
        {
            isPickingUpCrate = false;
        }
    }

    private void PickUpCrate()
    {
        if (!hasCrate)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, DetectionRadius);
            float closestDistance = Mathf.Infinity;
            Transform nearestObject = null;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Crate"))
                {
                    float distanceToCrate = Vector3.Distance(transform.position, collider.transform.position);
                    if (distanceToCrate < closestDistance)
                    {
                        closestDistance = distanceToCrate;
                        nearestObject = collider.transform;
                    }
                }
            }

            if (nearestObject != null)
            {
                nearestPickable = nearestObject.GetComponent<Pickup.IPickable>();
                if (nearestPickable != null && nearestPickable.StartPickup(gameObject))
                {
                    nearestObject.transform.parent = pickupPos;
                    nearestObject.transform.localPosition = Vector3.zero;
                    nearestObject.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    nearestPickable = null;
                }
            }
        }
    }

    private void OnPickUpCancel()
    {
        if (hasCrate)
        {
            if (nearestPickable != null)
            {
                pickupPos.DetachChildren();
                nearestPickable.ExitPickup();
                nearestPickable = null;
            }
        }

    }

    void RunAwayFromPlayer()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.PlayerTransform.position);

        // If the player is too close, run away from the player using NavMesh
        if (distanceToPlayer < safeDistance)
        {
            // Calculate the direction away from the player
            Vector3 runDirection = transform.position - GameManager.Instance.PlayerTransform.position;

            // Calculate the target position by adding the run direction to the enemy's position
            TargetPosition = transform.position + runDirection.normalized * safeDistance;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(TargetPosition, out hit, 5f, 1 << NavMesh.GetAreaFromName("Walkable")))
            {
                TargetPosition = hit.position;
            }
        }
        else
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f)
            {
                // Time to choose a new random destination.
                Wander();
                wanderTimer = WanderInterval;
            }
        }
    }

    public void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * WanderRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + randomDirection, out hit, WanderRadius, NavMesh.AllAreas);

        TargetPosition = hit.position;
    }
}
