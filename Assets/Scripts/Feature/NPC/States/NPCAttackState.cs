using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPCAttackState : NPCState
{
    private float RotationSpeed = 5f;
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
        RotateToTarget(RotationSpeed);
        if (npc.Agent.remainingDistance > npc.StopDistance)
        {
            npc.Agent.isStopped = false;
            npc.StateMachine.ChangeState(new EnemyMoveState(npc.GetComponent<NPC.NPC>(), npc.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void RotateToTarget(float rotationSpeed)
    {
        // Calculate the direction from this GameObject to the target
        Vector3 direction = npc.TargetPosition - npc.transform.position;
        direction.y = 0;

        // Create a rotation that looks in the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate towards the target rotation
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
