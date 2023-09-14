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
        Debug.Log("Initiating Enter state Companion Chase Enemy");
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
        if (currentChasedEnemy != null)
        {
            // Set the agent's destination to the position of the nearest enemy
            companion.UpdateAgentDestination(currentChasedEnemy);
            Debug.Log("Chasing enemy: " + currentChasedEnemy.name);
        }

        //If straying to far from the player, then chase the player instead
        float distanceToPlayer = Vector3.Distance(companion.transform.position, EnemySpawner.Instance.PlayerPosition.position);
        if (distanceToPlayer >= companion.DetectEnemyRadius)
        {
            companion.StateMachine.ChangeState(companion.CompanionChasePlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void HandleNearestEnemyChanged(GameObject newNearestEnemy)
    {
        Debug.Log("This event is called");
        // Do something with the new nearest enemy, such as updating AI behavior or targeting
        if (newNearestEnemy != null)
        {
            Debug.Log("Nearest enemy is not null");
            Debug.Log(newNearestEnemy.transform.position);
            // Set the current agent's destination to the position of the nearest enemy
            currentChasedEnemy = newNearestEnemy;
        }

        if(newNearestEnemy == null)
        {
            Debug.Log("New Nearest Enemy is Null");
        }
    }
}
