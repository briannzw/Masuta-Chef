using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : NPC, IWanderNPC, IDetectionNPC
{
    public float MaxDistanceFromPlayer;

    public virtual float WanderRadius { get; set; }
    public virtual float WanderInterval { get; set; }
    public float DetectionRadius { get; set; }
    public string TargetTag { get; set; }
    protected bool followEnemy = false;
    [HideInInspector]
    public float DistanceFromPlayer;
    protected Transform enemy;
    protected float wanderTimer = 0;
    protected bool shouldWander = false;

    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(NPCMoveState);
        DetectionRadius = 8f;
    }
    public void Attack()
    {

    }

    public void DetectTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(GameManager.playerTransform.position, DetectionRadius);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = collider.transform;
                    enemy = closestEnemy;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (Agent.remainingDistance <= StopDistance)
            {
                Agent.isStopped = true;
            }
            else
            {
                Agent.isStopped = false;
            }
            followEnemy = true;
            TargetPosition = closestEnemy.position;
        }
        else
        {
            followEnemy = false;
        }
    }

    public void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * WanderRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + randomDirection, out hit, WanderRadius, NavMesh.AllAreas);
        
        TargetPosition = hit.position;       
        
    }
    protected new void Update()
    {
        DistanceFromPlayer = Vector3.Distance(transform.position, GameManager.playerTransform.position);
        base.Update();
        if (followEnemy)
        {
            Agent.isStopped = false;
        }
        else if(!shouldWander)
        {
            TargetPosition = GameManager.playerTransform.position;

            if (Agent.remainingDistance <= MaxDistanceFromPlayer)
            {
                Agent.isStopped = true;
            }
            else
            {
                Agent.isStopped = false;
            }
        }

        if (DistanceFromPlayer > MaxDistanceFromPlayer)
        {
            StateMachine.ChangeState(NPCMoveState);
            followEnemy = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(TargetPosition, 0.5f);
    }
}
