using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackState : EnemyState
{
    Vector3 enemyPos;
    public EnemyRangedAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.ActiveWeapon.StartAttack();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.ActiveWeapon.StopAttack();
        enemy.AttackDuration = enemy.DefaultAttackDuration;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemyPos = enemy.transform.position;
        enemy.AttackDuration -= Time.deltaTime;
        if(enemy.AttackDuration <= 0f)
        {
            enemy.StateMachine.ChangeState(new EnemyRangedEngageState(enemy, enemy.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
