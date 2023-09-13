using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionChaseEnemyState : CompanionState
{
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
        Debug.Log("Companion Chase Enemy");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
