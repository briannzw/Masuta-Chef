using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPCAttackState : NPCState
{
    public NPCAttackState(NPC.NPC npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        npc.Agent.isStopped = true;
        npc.ActiveWeapon.StartAttack();
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.ActiveWeapon.StopAttack();
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
