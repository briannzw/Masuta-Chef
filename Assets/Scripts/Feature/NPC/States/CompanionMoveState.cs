using NPC.Companion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionMoveState : CompanionState
{
    public CompanionMoveState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
    {
    }

    public float DetectionRadius { get; set; }
    public string TargetTag { get; set; }

    public override void EnterState()
    {
        base.EnterState();
        companion.Agent.isStopped = false;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        companion.Agent.SetDestination(companion.TargetPosition);
        if (companion.IsFollowingEnemy && companion.Agent.remainingDistance <= companion.StopDistance)
        {
            companion.CompanionStateMachine.ChangeState(new CompanionAttackState(companion.GetComponent<Companion>(), companion.CompanionStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
