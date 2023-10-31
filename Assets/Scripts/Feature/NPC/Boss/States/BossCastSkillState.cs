using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCastSkillState : NPCState
{
    public BossCastSkillState(NPC.NPC npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        npc.Agent.isStopped = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.Agent.isStopped = false;
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
