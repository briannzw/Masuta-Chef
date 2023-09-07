using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeState : EnemyState
{
    private float timer = 0f;
    public EnemyMeleeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine){}

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
        timer += Time.deltaTime;
        Vector3 directionToPlayer = EnemySpawner.Instance.PlayerPosition.position - enemy.transform.position;
        enemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        if (timer >= enemy.MeleeInterval)
        {
            LaunchMeleeAttack();
            ResetTimer();
        }
        if (enemy.Agent.remainingDistance > enemy.MaxDistanceTowardsPlayer)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyChasePlayerState);
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
    private void ResetTimer()
    {
        // Reset the timer
        timer = 0f;
    }
    private void LaunchMeleeAttack()
    {

        // Do melee attack things
        Debug.Log("Melee Attack!");
    }


}
