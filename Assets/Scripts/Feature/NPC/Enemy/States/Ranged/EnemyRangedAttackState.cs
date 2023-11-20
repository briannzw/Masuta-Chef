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
        enemy.Animator.SetBool("IsAttacking", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.AttackDuration = enemy.DefaultAttackDuration;
        enemy.Animator.SetBool("IsAttacking", false);
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
        Vector3 direction = enemy.CurrentEnemies.transform.position - enemyPos;
        direction.y = 0;

        // Create a rotation that looks in the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate towards the target rotation
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
