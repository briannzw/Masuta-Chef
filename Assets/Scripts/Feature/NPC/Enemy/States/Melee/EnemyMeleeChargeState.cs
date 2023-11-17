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
        enemy.attackText.SetActive(true); //debug
        defaultSpeed = enemy.Agent.speed;
        newSpeed = enemy.Agent.speed + 1f;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.attackText.SetActive(false); //debug
        enemy.Agent.speed = defaultSpeed;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.Agent.SetDestination(enemy.CurrentEnemies.transform.position);
        enemy.Agent.speed = newSpeed;

        if (Vector3.Distance(enemy.transform.position, enemy.CurrentEnemies.transform.position) < enemy.AttackDistance)
        {
            enemy.StateMachine.ChangeState(new EnemyMeleeAttackState(enemy, enemy.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
