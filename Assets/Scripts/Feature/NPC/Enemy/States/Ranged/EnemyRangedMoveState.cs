using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedMoveState : EnemyState
{
    Vector3 playerPos;
    Vector3 enemyPos;
    public EnemyRangedMoveState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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
        playerPos = GameManager.Instance.PlayerTransform.position;
        enemyPos = enemy.transform.position;
        enemy.Agent.SetDestination(enemy.TargetPosition);

        if (Vector3.SqrMagnitude(playerPos - enemyPos) <= enemy.CombatEngageDistance)
        {
            enemy.StateMachine.ChangeState(new EnemyRangedEngageState(enemy, enemy.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
