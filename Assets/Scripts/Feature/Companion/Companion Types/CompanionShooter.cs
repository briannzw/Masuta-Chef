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
    private GameObject currentDestination;


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
        OnAgentDestinationChanged += HandleAgentDestinationChanged;
        StateMachine.Initialize(CompanionChasePlayerState);
        CompanionShootState = new CompanionShootState(this, StateMachine);
    }

    // Update is called once per frame
    private new void Update()
    {
        HandleStateMachine();
        CheckForEnemy();
        UpdateRotation();
        Agent.SetDestination(currentDestination.transform.position);
        DistanceToPlayer = Vector3.Distance(EnemySpawner.Instance.PlayerPosition.position, transform.position);
        Agent.speed = MoveSpeed;
    }

    private void OnDisable()
    {
        OnAgentDestinationChanged -= HandleAgentDestinationChanged;
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

        // If an enemy is detected, chase it
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
                    Debug.Log("Nearest enemy is unexpectedly null");
                }
            }
        }
    }

    private void HandleAgentDestinationChanged(GameObject newAgentDestination)
    {
        // Do something with the new nearest enemy, such as updating AI behavior or targeting
        if (newAgentDestination != null)
        {
            Debug.Log("New Agent Destination is not null");
            Debug.Log(newAgentDestination.transform.position);
            // Set the current agent's destination to the position of the new destination
            currentDestination = newAgentDestination;
        }

        if (newAgentDestination == null)
        {
            Debug.Log("New Agent Destination is Null");
        }
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
