using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionShooter : Companion
{
    [Header("Detection and Shooting")]
    [SerializeField] private float detectEnemyRadius = 10f; //The detect radius is from the player position
    [SerializeField] private float attackRange = 4f; //Max range to be able to shoot
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shootInterval;

    private Quaternion targetRotation;
    private GameObject thisNearestEnemy;


    #region Override Companion Variable
    public override float DetectEnemyRadius => detectEnemyRadius;
    public override float AttackRange => attackRange;
    public override GameObject Projectile => projectile;
    public override float ProjectileSpeed => projectileSpeed;
    public override float ShootInterval => shootInterval;
    #endregion

    public CompanionShootState CompanionShootState { get; set; }
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(CompanionChasePlayerState);
        CompanionShootState = new CompanionShootState(this, StateMachine);
    }

    // Update is called once per frame
    private new void Update()
    {
        HandleStateMachine();
        CheckForEnemy();
        UpdateRotation();

        Agent.speed = MoveSpeed;
    }

    private void HandleStateMachine()
    {
        StateMachine.CurrentCompanionState.FrameUpdate();
    }

    private void UpdateRotation()
    {
        Vector3 velocity = Agent.velocity;
        if (velocity.magnitude > 0)
        {
            targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * Agent.angularSpeed);
        }
    }

    private void CheckForEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(EnemySpawner.Instance.PlayerPosition.position, detectEnemyRadius);
        float nearestDistance = Mathf.Infinity;
        GameObject newNearestEnemy = null; // Initialize newNearestEnemy to null

        // Track whether an enemy is currently detected
        bool isEnemyDetected = false;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy")) //this line is null
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // Check if this is the nearest enemy inside the radius
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    newNearestEnemy = collider.gameObject;
                    isEnemyDetected = true;
                }
            }
        }

        // If an enemy is detected, chase it; otherwise, chase player
        if (isEnemyDetected)
        {
            // Only change state if the nearest enemy has changed
            if (newNearestEnemy != thisNearestEnemy)
            {
                thisNearestEnemy = newNearestEnemy;
                if (thisNearestEnemy != null)
                {
                    StateMachine.ChangeState(CompanionChaseEnemyState);
                    UpdateNearestEnemy(newNearestEnemy);
                    Debug.Log("Changing state to chasing enemy: " + thisNearestEnemy.name);
                }
                else
                {
                    // Handle the case where thisNearestEnemy is unexpectedly null
                    // You may want to add error handling or debug logs here.
                    Debug.Log("Nearest enemy is unexpectedly null");
                }
            }
            //    float enemyDistance = Vector3.Distance(transform.position, newNearestEnemy.transform.position);
            //    if (enemyDistance < attackRange && StateMachine.CurrentCompanionState == CompanionChaseEnemyState)
            //    {
            //        StateMachine.ChangeState(CompanionShootState);
            //        Vector3 directionToEnemy = newNearestEnemy.transform.position - transform.position;
            //        transform.rotation = Quaternion.LookRotation(directionToEnemy);
            //    }
            //    else
            //    {
            //        StateMachine.ChangeState(CompanionChaseEnemyState);
            //        Agent.isStopped = false;
            //        Agent.SetDestination(newNearestEnemy.transform.position);
            //    }

            //else
            //{
            //    // Handle the case where newNearestEnemy is unexpectedly null
            //    // You may want to add error handling or debug logs here.
            //}
        }
        //else
        //{
        //    StateMachine.ChangeState(CompanionChasePlayerState);
        //    Agent.SetDestination(EnemySpawner.Instance.PlayerPosition.position);
        //}
    }

    private void OnDrawGizmos()
    {
        DrawDetectionSphere();
    }

    private void DrawDetectionSphere()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(EnemySpawner.Instance.PlayerPosition.position, detectEnemyRadius);
    }
}
