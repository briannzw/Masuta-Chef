using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;
public class EnemyMoveState : NPCState
{
    public EnemyMoveState(NPC.NPC npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        npc.Agent.isStopped = false;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        npc.Agent.SetDestination(npc.TargetPosition);

        if (npc.Agent.remainingDistance <= npc.StopDistance && !npc.IsThisJoker)
        {
            npc.Agent.isStopped = true;
            npc.StateMachine.ChangeState(new NPCAttackState(npc.GetComponent<NPC.NPC>(), npc.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
