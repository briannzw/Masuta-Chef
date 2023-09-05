using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyState
{
    public float projectileSpeed = 10f;
    public float shootInterval = 2f;
    private float timer = 0f;
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
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
        // Calculate the direction from this object to the player
        Vector3 directionToPlayer = EnemySpawner.Instance.PlayerPosition.position - enemy.transform.position;

        // Make the object always face the player's direction
        enemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);

        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to shoot again
        if (timer >= shootInterval)
        {
            ShootProjectile();
            ResetTimer();
        }
        if (enemy.Agent.remainingDistance > enemy.MaxDistanceTowardsPlayer)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyChaseState);
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
        GameObject newProjectile = GameObject.Instantiate(enemy.Projectile, enemy.transform.position, Quaternion.identity);

        // Get the rigidbody of the projectile
        Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();

        // Check if the projectile has a rigidbody component
        if (projectileRigidbody != null)
        {
            // Set the velocity of the projectile to shoot it towards the player
            Vector3 directionToPlayer = EnemySpawner.Instance.PlayerPosition.position - enemy.transform.position;
            projectileRigidbody.velocity = directionToPlayer.normalized * projectileSpeed;
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
