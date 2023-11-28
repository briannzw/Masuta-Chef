using NPC.Companion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GourdakinMoveState : CompanionState
{
    public GourdakinMoveState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        companion.Animator.SetBool("IsRunning", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.Animator.SetBool("IsRunning", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        companion.Agent.SetDestination(companion.TargetPosition);

        if (companion.Agent.remainingDistance <= companion.StopDistance)
        {
            companion.CompanionStateMachine.ChangeState(new GourdakinIdleState(companion, companion.CompanionStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
