using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyJokerWanderState : EnemyState
{
    public EnemyJokerWanderState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        enemy.Agent.SetDestination(enemy.TargetPosition);


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
