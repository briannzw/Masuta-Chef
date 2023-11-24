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
        companion.Animator.SetBool("IsAttacking", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        companion.ActiveWeapon.StopAttack();
        companion.Animator.SetBool("IsAttacking", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        RotateToTarget(RotationSpeed);

        if(companion.CurrentEnemy != null && !companion.CurrentEnemy.GetComponent<NPC.Enemy.Enemy>().IsDead)
        {
            if(Vector3.SqrMagnitude(companion.CurrentEnemy.transform.position - companion.transform.position) > companion.AttackDistance)
            {
                companion.CompanionStateMachine.ChangeState(new CompanionMoveState(companion.GetComponent<Companion>(), companion.CompanionStateMachine));
            }
        }

        if(Vector3.SqrMagnitude(GameManager.Instance.PlayerTransform.position - companion.transform.position) > companion.MaxDistanceFromPlayer)
        {
            companion.CompanionStateMachine.ChangeState(new CompanionMoveState(companion.GetComponent<Companion>(), companion.CompanionStateMachine));
        }

        if (companion.CurrentEnemy.GetComponent<NPC.Enemy.Enemy>().IsDead)
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
