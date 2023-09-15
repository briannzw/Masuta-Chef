using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCloseAttacker : Companion
{
    [SerializeField] private float detectEnemyRadius = 10f;
    [SerializeField] private float attackRange = 4f;
    public override Transform TargetDestination => EnemySpawner.Instance.PlayerPosition;
    public override float DetectEnemyRadius => detectEnemyRadius;
    public override float AttackRange => attackRange;

    private GameObject nearestEnemy;

    public CompanionCloseAttackState CompanionCloseAttackState { get; set; }

    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(CompanionChasePlayerState);
        CompanionCloseAttackState = new CompanionCloseAttackState(this, StateMachine);
    }

    private new void Update()
    {
        StateMachine.CurrentCompanionState.FrameUpdate();
        CheckForEnemy();
        if (MaxIdleDistance > detectEnemyRadius)
        {
            StateMachine.ChangeState(CompanionChasePlayerState);
        }
        if(Agent.remainingDistance <= 3f)
        {
            StateMachine.ChangeState(CompanionIdleState);
        }
        Agent.speed = MoveSpeed;
        Vector3 velocity = Agent.velocity;
        if (velocity.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * Agent.angularSpeed);
        }
    }

    private void CheckForEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(EnemySpawner.Instance.PlayerPosition.position, detectEnemyRadius);
        float nearestDistance = Mathf.Infinity;
        float playerDistance = Vector3.Distance(transform.position, EnemySpawner.Instance.PlayerPosition.position);
        GameObject nearestEnemy = null; // Initialize nearestEnemy to null

        // Track whether an enemy is currently detected
        bool isEnemyDetected = false;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // Check if this is the nearest enemy inside the radius
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = collider.gameObject;
                    isEnemyDetected = true;
                }
            }
        }

        // If an enemy is detected, chase it; otherwise, chase player
        if (isEnemyDetected && playerDistance < detectEnemyRadius)
        {
            float enemyDistance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            if (enemyDistance < attackRange && StateMachine.CurrentCompanionState == CompanionChaseEnemyState)
            {
                StateMachine.ChangeState(CompanionCloseAttackState);
            }
            else
            {
                StateMachine.ChangeState(CompanionChaseEnemyState);
                Agent.isStopped = false;
                Agent.SetDestination(nearestEnemy.transform.position);
            }
        }
        else
        {
            StateMachine.ChangeState(CompanionChasePlayerState);
            Agent.SetDestination(EnemySpawner.Instance.PlayerPosition.position);
        }
    }

    private void OnDrawGizmos()
    {
        // Set the color of the Gizmo sphere
        Gizmos.color = Color.red;

        // Draw a wire sphere at the Player's position with the specified detectionRadius
        Gizmos.DrawWireSphere(EnemySpawner.Instance.PlayerPosition.position, detectEnemyRadius);
    }
}
