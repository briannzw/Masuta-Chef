using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(NPCMoveState);
    }

    protected new void Update()
    {
        base.Update();
        TargetPosition = GameManager.playerTransform.position;

        if (Agent.remainingDistance <= StopDistance)
        {
            Agent.isStopped = true;
        }
        else
        {
            Agent.isStopped = false;
        }
    }
}
