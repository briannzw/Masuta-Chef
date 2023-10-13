using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class Enemy : NPC
{

    public bool IsTaunted = false;
    private float currentTauntTimer;
    private Character.Character chara;
    [SerializeField] private float maxTauntTimer = 0.1f;
    [SerializeField] float rotationSpeed = 5.0f;
    
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(NPCMoveState);
        currentTauntTimer = maxTauntTimer;
        chara = GetComponentInChildren<Character.Character>();
        chara.OnDie += EnemyDie;
    }

    private void EnemyDie()
    {
        Destroy(gameObject);
    }

    protected new void Update()
    {
        
        //Debug.Log("Is Taunted: " + IsTaunted);
        base.Update();
        currentTauntTimer -= Time.deltaTime;
        if (!IsTaunted)
        {
            TargetPosition = GameManager.playerTransform.position;
        }

        if (Agent.remainingDistance <= StopDistance)
        {
            Agent.isStopped = true;
            StateMachine.ChangeState(NPCAttackState);
            RotateToTarget(rotationSpeed);
        }
        else
        {
            Agent.isStopped = false;
            StateMachine.ChangeState(NPCMoveState);
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
        direction.y = 0;

        // Create a rotation that looks in the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
