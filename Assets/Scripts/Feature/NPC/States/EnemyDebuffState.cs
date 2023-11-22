using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDebuffState : EnemyState
{
    private float wanderRadius = 7f;

    public EnemyDebuffState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.Agent.isStopped = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.IsStun)
        {
            enemy.Agent.isStopped = true;
        }
        else
        {
            enemy.StateMachine.ChangeState(new EnemyMoveState(enemy.GetComponent<NPC.Enemy.Enemy>(), enemy.StateMachine));
        }
        if (enemy.IsConfused)
        {
            Wander();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(enemy.transform.position + randomDirection, out hit, wanderRadius, NavMesh.AllAreas);
        enemy.Agent.SetDestination(hit.position);
    }
}
