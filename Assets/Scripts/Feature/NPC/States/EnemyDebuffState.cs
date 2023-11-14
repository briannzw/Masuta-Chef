using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDebuffState : NPCState
{
    private float wanderRadius = 7f;
    public EnemyDebuffState(NPC.NPC npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.Agent.isStopped = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        //if (stun)
        //{
        //  npc.Agent.isStopped = true;
        //}
        //else
        //{
        //npc.StateMachine.ChangeState(new EnemyMoveState(npc.GetComponent<NPC.NPC>(), npc.StateMachine));
        //}
        //if (confused)
        //{
        //  Wander();
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(npc.transform.position + randomDirection, out hit, wanderRadius, NavMesh.AllAreas);
        npc.Agent.SetDestination(hit.position);
    }
}
