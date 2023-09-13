using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCloseAttackState : CompanionState
{
    private float timer = 0f;
    public CompanionCloseAttackState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Companion.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Companion Close Attack");
        companion.Agent.isStopped = true;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        timer += Time.deltaTime;
        if (timer >= companion.AttackInterval)
        {
            LaunchMeleeAttack();
            ResetTimer();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void InitializeTimer()
    {
        // Initialize the timer
        timer = 0f;
    }
    private void ResetTimer()
    {
        // Reset the timer
        timer = 0f;
    }
    private void LaunchMeleeAttack()
    {

        // Do melee attack things
        Debug.Log("Melee Attack!");
    }
}
