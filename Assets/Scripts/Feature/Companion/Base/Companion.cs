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
    [field: SerializeField] public float MaxIdleDistance { get; set; } = 10f;
    [field: SerializeField] public float MinDistanceTowardsPlayer { get; set; } = 3f;
    [field: SerializeField] public float MoveSpeed { get; set; } = 3.25f;
    public virtual CompanionState CompanionCombatBehaviour { get; set; }
    public virtual float DetectEnemyRadius { get; set; } = 10f;
    [field: SerializeField] public virtual float AttackRange { get; set; }
    public virtual float AttackInterval { get; set; }

    [HideInInspector]
    public float DistanceToPlayer { get; set; }

    #region State Machine Variables
    public CompanionStateMachine StateMachine { get; set; }
    public CompanionChasePlayerState CompanionChasePlayerState { get; set; }
    public CompanionIdleState CompanionIdleState { get; set; }
    public CompanionChaseEnemyState CompanionChaseEnemyState { get; set; }
    public CompanionShootState CompanionShootState { get; set; }

    #endregion

    #region Companion Shoot Variable
    public virtual GameObject Projectile { get; set; }
    public virtual float ProjectileSpeed { get; set; } = 14f;
    public virtual float ShootInterval { get; set; } = 1f;
    #endregion

    #region Nearest Enemy Delegate and Update
    public event System.Action<GameObject> OnNearestEnemyChanged;

    private GameObject nearestEnemy;

    public GameObject NearestEnemy
    {
        get { return nearestEnemy; }
        private set
        {
            if (nearestEnemy != value)
            {
                nearestEnemy = value;
                OnNearestEnemyChanged?.Invoke(nearestEnemy);
            }
        }
    }

    public void UpdateNearestEnemy(GameObject newNearestEnemy)
    {
        NearestEnemy = newNearestEnemy;
    }
    #endregion

    #region Agent Destination Delegate and Update
    public event System.Action<GameObject> OnAgentDestinationChanged;

    private GameObject agentDestination;

    public GameObject AgentDestination
    {
        get { return agentDestination; }
        private set
        {
            if (agentDestination != value)
            {
                agentDestination = value;
                OnAgentDestinationChanged?.Invoke(agentDestination);
            }
        }
    }

    public void UpdateAgentDestination (GameObject newAgentDestination)
    {
        AgentDestination = newAgentDestination;
    }
    #endregion
    protected void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        StateMachine = new CompanionStateMachine();
        CompanionChasePlayerState = new CompanionChasePlayerState(this, StateMachine);
        CompanionIdleState = new CompanionIdleState(this, StateMachine);
        CompanionChaseEnemyState = new CompanionChaseEnemyState(this, StateMachine);
        CompanionShootState = new CompanionShootState(this, StateMachine);


        CurrentHealth = MaxHealth;

    }

        // Update is called once per frame
    protected void Update()
    {
        StateMachine.CurrentCompanionState.FrameUpdate();
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
