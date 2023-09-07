using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShootState : EnemyState
{
    private float timer = 0f;
    public EnemyShootState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

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
        Vector3 directionToPlayer = EnemySpawner.Instance.PlayerPosition.position - enemy.transform.position;
        enemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        timer += Time.deltaTime;
        if (timer >= enemy.ShootInterval)
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
            projectileRigidbody.velocity = directionToPlayer.normalized * enemy.ProjectileSpeed;
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
