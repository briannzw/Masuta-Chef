using NPC.Companion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionDeadState : CompanionState
{
    public CompanionDeadState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        base.EnterState();
        companion.Agent.isStopped = true;
        companion.ChildCollider.enabled = false;
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
