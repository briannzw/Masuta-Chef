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
        if(npc.ActiveWeapon != null) npc.ActiveWeapon.StartAttack();
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
