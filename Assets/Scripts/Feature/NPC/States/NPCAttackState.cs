using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackState : NPCState
{
    public NPCAttackState(NPC npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine)
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
        if(npc.Agent.remainingDistance > npc.StopDistance)
        {
            npc.StateMachine.ChangeState(npc.NPCMoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
