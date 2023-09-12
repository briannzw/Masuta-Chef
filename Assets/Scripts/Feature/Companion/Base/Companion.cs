using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth { get; set; }
    public virtual Transform TargetDestination { get; set; }
    public NavMeshAgent Agent { get; set; }
    [field: SerializeField] public float MaxDistanceTowardsPlayer { get; set; } = 2f;
    [field: SerializeField] public float MoveSpeed { get; set; } = 3.25f;
    public virtual CompanionState CompanionCombatBehaviour { get; set; }
    public virtual float DetectEnemyRadius { get; set; } = 10f;
    // Start is called before the first frame update

    #region State Machine Variables
    public CompanionStateMachine StateMachine { get; set; }
    public CompanionChasePlayerState CompanionChasePlayerState { get; set; }
    public CompanionIdleState CompanionIdleState { get; set; }
    #endregion

    protected void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        StateMachine = new CompanionStateMachine();
        CompanionChasePlayerState = new CompanionChasePlayerState(this, StateMachine);
        CompanionIdleState = new CompanionIdleState(this, StateMachine);

        CurrentHealth = MaxHealth;

    }

        // Update is called once per frame
    protected void Update()
    {
        Agent.SetDestination(TargetDestination.position);
        StateMachine.CurrentCompanionState.FrameUpdate();
        Agent.speed = MoveSpeed;
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentCompanionState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        CompanionDamaged,
        PlayFootstepSound,

    }
}
