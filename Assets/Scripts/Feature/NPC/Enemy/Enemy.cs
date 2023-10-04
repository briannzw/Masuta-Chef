using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    public bool IsTaunted = false;
    private float currentTauntTimer;
    private float maxTauntTimer = 5f;
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(NPCMoveState);
        currentTauntTimer = maxTauntTimer;
    }

    protected new void Update()
    {
        Debug.Log("Is Taunted: " + IsTaunted);
        base.Update();
        if(!IsTaunted)
            TargetPosition = GameManager.playerTransform.position;

        if (Agent.remainingDistance <= StopDistance)
        {
            Agent.isStopped = true;
        }
        else
        {
            Agent.isStopped = false;
        }

        if (IsTaunted)
        {
            currentTauntTimer -= Time.deltaTime;
            if (currentTauntTimer <= 0f)
            {
                RemoveTauntEffect();
                currentTauntTimer = maxTauntTimer;
            }
        }
    }

    void RemoveTauntEffect()
    {
        IsTaunted = false;
    }
}
