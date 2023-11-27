using NPC.Companion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionIdleState : CompanionState
{
    public CompanionIdleState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
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
        if (Vector3.SqrMagnitude(companion.transform.position - GameManager.Instance.PlayerTransform.position) > companion.MaxDistanceFromPlayer)
        {
            companion.CompanionStateMachine.ChangeState(new CompanionMoveState(companion, companion.CompanionStateMachine));
        }

        if (companion.IsFollowingEnemy)
        {
            companion.CompanionStateMachine.ChangeState(new CompanionMoveState(companion, companion.CompanionStateMachine));
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
