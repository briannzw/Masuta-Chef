using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeChargeState : EnemyState
{
    private float defaultSpeed;
    private float newSpeed;
    public EnemyMeleeChargeState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.stoppingDistance = enemy.AttackDistance - 0.1f;
        defaultSpeed = enemy.Agent.speed;
        newSpeed = enemy.Agent.speed + 2f;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.Agent.speed = defaultSpeed;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.Agent.SetDestination(enemy.CurrentEnemy.transform.position);
        enemy.Agent.speed = newSpeed;

        if (Vector3.Distance(enemy.transform.position, enemy.CurrentEnemy.transform.position) < enemy.AttackDistance)
        {
            enemy.StateMachine.ChangeState(new EnemyMeleeAttackState(enemy, enemy.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
