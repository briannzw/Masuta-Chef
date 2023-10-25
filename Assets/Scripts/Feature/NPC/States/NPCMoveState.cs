using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoveState : NPCState
{
    public NPCMoveState(NPC.NPC npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        npc.Agent.SetDestination(npc.TargetPosition);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
