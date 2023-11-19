using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyJokerWanderState : EnemyState
{
    private Vector3 randomDirection;
    private float wanderRadius;
    public EnemyJokerWanderState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        wanderRadius = enemy.GetComponent<JokerEnemy>().WanderRadius;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        randomDirection = Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + randomDirection, out hit, wanderRadius, NavMesh.AllAreas);

        enemy.CurrentEnemies.transform.position = hit.position;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
