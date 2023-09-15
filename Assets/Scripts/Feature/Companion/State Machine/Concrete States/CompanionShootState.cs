using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionShootState : CompanionState
{
    private float timer = 0f;
    public CompanionShootState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Companion.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        companion.Agent.isStopped = true;
        InitializeTimer();
    }

    public override void ExitState()
    {
        base.ExitState();
        ResetTimer();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        float distanceToEnemy = Vector3.Distance(companion.transform.position, CompanionScanEnemy.currentNearestEnemy.transform.position);     
        timer += Time.deltaTime;
        Vector3 directionToEnemy = CompanionScanEnemy.currentNearestEnemy.transform.position - companion.transform.position;
        companion.transform.rotation = Quaternion.LookRotation(directionToEnemy);
        if (timer >= companion.ShootInterval && distanceToEnemy < companion.AttackRange)
        {
            ShootProjectile();
            ResetTimer();
        }
        if (distanceToEnemy > companion.AttackRange)
        {
            companion.StateMachine.ChangeState(companion.CompanionChaseEnemyState);
        }
        
        if (Vector3.Distance(companion.transform.position, GameManager.Instance.PlayerGameObject.transform.position) > companion.DetectEnemyRadius)
        {
            companion.StateMachine.ChangeState(companion.CompanionChasePlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void InitializeTimer()
    {
        // Initialize the timer
        timer = 0f;
    }
    private void ShootProjectile()
    {
        // Spawn a new projectile
        GameObject newProjectile = GameObject.Instantiate(companion.Projectile, companion.transform.position, Quaternion.identity);

        // Get the rigidbody of the projectile
        Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();

        // Check if the projectile has a rigidbody component
        if (projectileRigidbody != null)
        {
            // Set the velocity of the projectile to shoot it towards the player
            Vector3 directionToEnemy = CompanionScanEnemy.currentNearestEnemy.transform.position - companion.transform.position;
            projectileRigidbody.velocity = directionToEnemy.normalized * companion.ProjectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile prefab should have a Rigidbody component.");
        }
    }

    private void ResetTimer()
    {
        // Reset the timer
        timer = 0f;
    }
}
