using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Chassiiing!");
        enemy.Agent.isStopped = false;
        enemy.Agent.destination = enemy.TargetDestination.position;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.Agent.isStopped = true;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        // Get the velocity vector from the NavMesh Agent
        Vector3 velocity = enemy.Agent.velocity;

        // Check if there is any movement (magnitude > 0)
        if (velocity.magnitude > 0)
        {
            // Calculate the rotation angle
            Quaternion targetRotation = Quaternion.LookRotation(velocity);

            // Smoothly rotate towards the target rotation
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * enemy.Agent.angularSpeed);
        }
        if (enemy.Agent.remainingDistance < enemy.MaxDistanceTowardsPlayer)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}