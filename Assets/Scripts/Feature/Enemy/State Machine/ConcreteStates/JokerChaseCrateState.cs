using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerChaseCrateState : EnemyState
{
    public JokerChaseCrateState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine){}

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.SetDestination(enemy.TargetDestination.position);
        enemy.Agent.isStopped = false;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.Agent.isStopped = true;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
