using NPC.Companion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAttackState : CompanionState
{
    private float RotationSpeed = 5f;

    public CompanionAttackState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        companion.Agent.isStopped = true;
        companion.ActiveWeapon.StartAttack();
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.ActiveWeapon.StopAttack();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        companion.Agent.SetDestination(companion.TargetPosition);
        RotateToTarget(RotationSpeed);
        if (companion.DistanceFromPlayer > companion.MaxDistanceFromPlayer)
        {
            companion.CompanionStateMachine.ChangeState(new CompanionMoveState(companion.GetComponent<Companion>(), companion.CompanionStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void RotateToTarget(float rotationSpeed)
    {
        // Calculate the direction from this GameObject to the target
        Vector3 direction = companion.TargetPosition - companion.transform.position;
        direction.y = 0;

        // Create a rotation that looks in the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate towards the target rotation
        companion.transform.rotation = Quaternion.Slerp(companion.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
