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
        enemy.Agent.isStopped = true;
        enemy.ActiveWeapon.StartAttack();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.Agent.isStopped = false;
        enemy.AttackDuration = enemy.DefaultAttackDuration;
        enemy.ActiveWeapon.StopAttack();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemyPos = enemy.transform.position;
        enemy.AttackDuration -= Time.deltaTime;
        RotateToTarget(20f);

        if(enemy.AttackDuration <= 0f)
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
        // Calculate the direction from this GameObject to the target
        Vector3 direction = enemy.CurrentEnemy.transform.position - enemyPos;
        direction.y = 0;

        // Create a rotation that looks in the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate towards the target rotation
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
