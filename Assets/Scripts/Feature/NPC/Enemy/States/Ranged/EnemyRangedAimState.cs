using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAimState : EnemyState
{
    private float defaultAttackTimer;
    private Vector3 enemyPos;
    public EnemyRangedAimState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        defaultAttackTimer = enemy.AttackTimer;
        enemy.Agent.isStopped = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.AttackTimer = defaultAttackTimer;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemyPos = enemy.transform.position;
        enemy.AttackTimer -= Time.deltaTime;
        RotateToTarget(20f);
        if(enemy.AttackTimer <= 0f)
        {
            enemy.StateMachine.ChangeState(new EnemyRangedAttackState(enemy, enemy.StateMachine));
        }

        if(Vector3.Distance(enemy.CurrentEnemies.transform.position, enemyPos) > enemy.AttackDistance + 4f)
        {
            enemy.StateMachine.ChangeState(new EnemyRangedEngageState(enemy, enemy.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void RotateToTarget(float rotationSpeed)
    {
        Vector3 direction = enemy.CurrentEnemies.transform.position - enemyPos;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
