using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JokerEnemy : Enemy, IDetectionNPC, IWanderNPC
{
    [field: SerializeField] public float DetectionRadius { get; set; }
    public string TargetTag { get; set; }
    [field:SerializeField] public float WanderRadius { get; set; }
    [field: SerializeField] public float WanderInterval { get; set; }
    [SerializeField] float wanderTimer = 0;
    private bool isPickingUpCrate = false;

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
        }
        else
        {
            isPickingUpCrate = false;
        }
    }

    public void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * WanderRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + randomDirection, out hit, WanderRadius, NavMesh.AllAreas);

        TargetPosition = hit.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        if (!isPickingUpCrate)
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
}
