using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChasePlayerState : EnemyState
{
    public EnemyChasePlayerState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) { }

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
        Vector3 velocity = enemy.Agent.velocity;
        if (velocity.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * enemy.Agent.angularSpeed);
        }
        if (enemy.Agent.remainingDistance < enemy.MaxDistanceTowardsPlayer)
        {
            enemy.StateMachine.ChangeState(enemy.EnemyCombatBehaviour);
        }
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
