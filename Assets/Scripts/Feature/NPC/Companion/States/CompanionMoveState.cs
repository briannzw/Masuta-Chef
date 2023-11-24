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

        if(companion.IsFollowingEnemy && companion.Agent.remainingDistance <= companion.AttackDistance - 0.1f)
        {
            companion.CompanionStateMachine.ChangeState(new CompanionAttackState(companion, companion.CompanionStateMachine));
        }

        if(!companion.IsFollowingEnemy && companion.Agent.remainingDistance <= companion.StopDistance + 0.1f)
        {
            companion.CompanionStateMachine.ChangeState(new CompanionIdleState(companion, companion.CompanionStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
