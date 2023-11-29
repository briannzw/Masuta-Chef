using NPC.Companion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GourdakinWanderState : CompanionState
{
    private Transform sphereCenter;
    private bool isMoving = false;
    private bool isTaunting = false; // Added boolean for taunt animation
    private float timer = 0f;
    private float delay = 0f;
    private float tauntTimer = 0f;
    private float tauntDuration = 2f; // Adjust the taunt duration as needed

    private Vector3 newTargetPos;

    public GourdakinWanderState(Companion companion, CompanionStateMachine companionStateMachine) : base(companion, companionStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        SetRandomTarget();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        sphereCenter = GameManager.Instance.PlayerTransform;
        companion.Animator.SetBool("IsTaunting", isTaunting);
        companion.Animator.SetBool("IsRunning", isMoving);
        companion.Agent.SetDestination(newTargetPos);
        if (!companion.Agent.pathPending && !isMoving)
        {
            if (Time.time - timer > delay)
            {
                isMoving = false;
                if (!isTaunting)
                {
                    isTaunting = true;
                }

                if (Time.time - tauntTimer > tauntDuration)
                {
                    tauntTimer = Time.time;
                    SetRandomTarget();
                    isTaunting = false; // Reset the taunt flag after setting a new target
                }

                timer = Time.time;
                delay = Random.Range(5f, 8f);
            }
        }

        if (!companion.IsFollowingEnemy)
        {
            companion.CompanionStateMachine.ChangeState(new GourdakinIdleState(companion, companion.CompanionStateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void SetRandomTarget()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 10f; // Adjust the radius as needed
        Vector3 sphereCenterPosition = sphereCenter.position;
        randomPoint += sphereCenterPosition; // Offset the random point by the sphere's center
        NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 10f, NavMesh.AllAreas);
        newTargetPos = hit.position;
        isMoving = true; // Set to true when moving to prevent setting a new target immediately
    }
}
