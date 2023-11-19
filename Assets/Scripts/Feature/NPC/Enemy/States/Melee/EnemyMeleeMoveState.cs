using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeMoveState : EnemyState
{
    Vector3 currentEnemyPos;
    Vector3 agentPos;

    public EnemyMeleeMoveState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.isStopped = false;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        currentEnemyPos = enemy.CurrentEnemies.transform.position;
        agentPos = enemy.transform.position;
        enemy.Agent.SetDestination(currentEnemyPos);

        if (Vector3.SqrMagnitude(currentEnemyPos - agentPos) <= enemy.CombatEngageDistance && !enemy.IsThisJoker)
        {
            enemy.StateMachine.ChangeState(new EnemyMeleeEngageState(enemy, enemy.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
