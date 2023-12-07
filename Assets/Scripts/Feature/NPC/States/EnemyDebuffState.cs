using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDebuffState : EnemyState
{
    private float wanderRadius = 10f;
    private float confusedTimer;
    private float wanderInterval = 2f;

    public EnemyDebuffState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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
        enemy.Agent.isStopped = false;
        enemy.IsDebuffed = false;
        enemy.StunIcon.gameObject.SetActive(false);
        enemy.ConfusedIcon.gameObject.SetActive(false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.IsStun)
        {
            enemy.Agent.isStopped = true;
            enemy.StunIcon.gameObject.SetActive(true);
        }
        else if (enemy.IsConfused)
        {
            enemy.ConfusedIcon.gameObject.SetActive(true);
            confusedTimer -= Time.deltaTime;
            if (confusedTimer <= 0f)
            {
                // Time to choose a new random destination.
                Wander();
                confusedTimer = wanderInterval;
            }
        }
        else
        {
            enemy.StateMachine.ChangeState(new EnemyMoveState(enemy.GetComponent<NPC.Enemy.Enemy>(), enemy.StateMachine));
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
