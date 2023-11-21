using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttackState : EnemyState
{
    public EnemyMeleeAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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
        RotateToTarget(15f);
        enemy.AttackDuration -= Time.deltaTime;
        if(Vector3.Distance(enemy.transform.position, enemy.CurrentEnemies.transform.position) > enemy.AttackDistance || enemy.AttackDuration <= 0)
        {
            enemy.StateMachine.ChangeState(new EnemyMeleeEngageState(enemy, enemy.StateMachine));
        }

        enemy.Agent.SetDestination(enemy.CurrentEnemies.transform.position);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void RotateToTarget(float rotationSpeed)
    {
        Vector3 direction = enemy.CurrentEnemies.transform.position - enemy.transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
