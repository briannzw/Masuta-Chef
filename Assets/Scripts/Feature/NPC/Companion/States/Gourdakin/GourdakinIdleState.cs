using NPC.Companion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GourdakinIdleState : CompanionState
{
    public GourdakinIdleState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        companion.Agent.isStopped = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.Agent.isStopped = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        //if (companion.IsFollowingEnemy)
        //{
        //    companion.CompanionStateMachine.ChangeState(new GourdakinWanderState(companion, companionStateMachine));
        //}

        if (Vector3.Distance(companion.transform.position, GameManager.Instance.PlayerTransform.position) > companion.MaxDistanceFromPlayer)
        {
            companion.CompanionStateMachine.ChangeState(new GourdakinMoveState(companion, companion.CompanionStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
