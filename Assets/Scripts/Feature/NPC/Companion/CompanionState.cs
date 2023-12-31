using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionState
{
    protected NPC.Companion.Companion companion;
    protected CompanionStateMachine companionStateMachine;

    public CompanionState(NPC.Companion.Companion companion, CompanionStateMachine companionStateMachine)
    {
        this.companion = companion;
        this.companionStateMachine = companionStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
}
