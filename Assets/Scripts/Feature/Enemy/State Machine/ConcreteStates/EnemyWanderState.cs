using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyWanderState : EnemyState
{
    private float timer;
    public EnemyWanderState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine){}

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.isStopped = false;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.Agent.isStopped = true;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (!enemy.Agent.isStopped)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                // Time to choose a new random destination.
                SetNewRandomDestination();
                timer = enemy.WanderTimer;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void SetNewRandomDestination()
    {
        // Generate a random position within the wander radius.
        Vector3 randomDirection = Random.insideUnitSphere * enemy.WanderRadius;

        // Project the position onto the NavMesh to find a valid destination.
        NavMeshHit hit;
        NavMesh.SamplePosition(enemy.transform.position + randomDirection, out hit, enemy.WanderRadius, NavMesh.AllAreas);

        // Set the new target position.
        enemy.Agent.SetDestination(hit.position);
    }
}
