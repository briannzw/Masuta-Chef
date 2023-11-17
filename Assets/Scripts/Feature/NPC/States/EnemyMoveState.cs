using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;
using NPC.Enemy;

public class EnemyMoveState : EnemyState
{
    Vector3 playerPos;
    Vector3 enemyPos;

    public EnemyMoveState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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

        //if (Vector3.SqrMagnitude(playerPos - enemyPos) <= stopDistanceSquared && !enemy.IsThisJoker)
        //{
        //    enemy.Agent.isStopped = true;
        //    enemy.StateMachine.ChangeState(new NPCAttackState(enemy.GetComponent<NPC.NPC>(), enemy.StateMachine));
        //}
        if (Vector3.SqrMagnitude(playerPos - enemyPos) <= enemy.CombatEngageDistance && !enemy.IsThisJoker)
        {
            enemy.StateMachine.ChangeState(new EnemyMeleeEngageState(enemy.GetComponent<NPC.Enemy.Enemy>(), enemy.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
