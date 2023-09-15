using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionChaseEnemyState : CompanionState
{
    private GameObject currentChasedEnemy;
    public CompanionChaseEnemyState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Companion.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        companion.Agent.isStopped = false;
        companion.OnNearestEnemyChanged += HandleNearestEnemyChanged; //Subscribe to the event of nearest enemy
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.Agent.isStopped = true;
        companion.OnNearestEnemyChanged -= HandleNearestEnemyChanged; //Unsubscribe from the event of nearest enemy
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        companion.UpdateAgentDestination(CompanionScanEnemy.currentNearestEnemy);
        float distanceToPlayer = Vector3.Distance(companion.transform.position, GameManager.Instance.PlayerGameObject.transform.position);
        if (distanceToPlayer >= companion.DetectEnemyRadius)
        {
            companion.StateMachine.ChangeState(companion.CompanionChasePlayerState);
            Debug.Log("From chasing enemy to player");
        }
        if(companion.CompanionCombatBehaviour == companion.CompanionShootState && companion.Agent.remainingDistance < companion.AttackRange)
        {
            companion.StateMachine.ChangeState(companion.CompanionShootState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void HandleNearestEnemyChanged(GameObject newNearestEnemy)
    {
        if (newNearestEnemy != null)
        {
            currentChasedEnemy = newNearestEnemy;
        }
    }
}
