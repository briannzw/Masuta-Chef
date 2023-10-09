using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    public bool IsTaunted = false;
    private float currentTauntTimer;
    [SerializeField] private float maxTauntTimer = 0.1f;
    [SerializeField] float rotationSpeed = 5.0f;
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(NPCMoveState);
        currentTauntTimer = maxTauntTimer;
    }

    protected new void Update()
    {
        //Debug.Log("Is Taunted: " + IsTaunted);
        base.Update();
        RotateToTarget(rotationSpeed);
        currentTauntTimer -= Time.deltaTime;
        if (!IsTaunted)
        {
            TargetPosition = GameManager.playerTransform.position;
        }
            

        if (Agent.remainingDistance <= StopDistance)
        {
            Agent.isStopped = true;
        }
        else
        {
            Agent.isStopped = false;
        }

        if (IsTaunted && Agent.remainingDistance > 5f || IsTaunted && currentTauntTimer <= 0f)
        {
            RemoveTauntEffect();
        }
    }

    void RemoveTauntEffect()
    {
        IsTaunted = false;
        Agent.isStopped = false;
        currentTauntTimer = maxTauntTimer;
    }

    protected void RotateToTarget(float rotationSpeed)
    {
        // Calculate the direction from this GameObject to the target
        Vector3 direction = TargetPosition - transform.position;

        // Create a rotation that looks in the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
