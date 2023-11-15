using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;
public class EnemyMoveState : NPCState
{
    Vector3 playerPos;
    Vector3 npcPos;
    float stopDistanceSquared;
    public EnemyMoveState(NPC.NPC npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine)
    {
        stopDistanceSquared = npc.StopDistance * npc.StopDistance;
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
        playerPos = GameManager.Instance.PlayerTransform.position;
        npcPos = npc.transform.position;
        npc.Agent.SetDestination(npc.TargetPosition);

        //if (Vector3.SqrMagnitude(playerPos - npcPos) <= stopDistanceSquared && !npc.IsThisJoker)
        //{
        //    npc.Agent.isStopped = true;
        //    npc.StateMachine.ChangeState(new NPCAttackState(npc.GetComponent<NPC.NPC>(), npc.StateMachine));
        //}
        if (Vector3.SqrMagnitude(playerPos - npcPos) <= npc.CombatEngageDistance && !npc.IsThisJoker)
        {
            npc.StateMachine.ChangeState(new EnemyEngageState(npc.GetComponent<NPC.NPC>(), npc.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
