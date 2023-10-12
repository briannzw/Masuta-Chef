using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Character;
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
    public Transform companionSlotPosition;
    [SerializeField] protected float minDistanceFromPlayer;

    [SerializeField] private float minDistanceSlotFromPlayer;

    private Character.Character chara;
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(NPCMoveState);
        DetectionRadius = 8f;
        chara = GetComponent<Character.Character>();
        chara.OnDie += CompanionDie;
    }
    public void Attack()
    {

    }

    private void CompanionDie()
    {
        Destroy(gameObject);
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
                StateMachine.ChangeState(NPCAttackState);
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
        NavMeshHit hit;

        // Determine the target position based on different conditions
        Vector3 targetPosition;
        if (followEnemy)
        {
            targetPosition = TargetPosition;
            Agent.isStopped = false;
        }
        else
        {
            NavMesh.SamplePosition(companionSlotPosition.position, out hit, Mathf.Infinity, 1 << NavMesh.GetAreaFromName("Walkable"));
            float distanceSlot = Mathf.Abs(hit.position.y - companionSlotPosition.position.y);
            targetPosition = (distanceSlot < 1f) ? hit.position : TargetPosition;
            Agent.isStopped = Agent.remainingDistance <= minDistanceFromPlayer;
        }

        TargetPosition = targetPosition;

        // Change state based on distance from player
        if (DistanceFromPlayer > MaxDistanceFromPlayer)
        {
            TargetPosition = GameManager.playerTransform.position;
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
