using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEnemyDamageable, IEnemyMoveable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public virtual Transform TargetDestination { get; set; }
    public NavMeshAgent Agent { get; set; }
    [field: SerializeField] public float MaxDistanceTowardsPlayer { get; set; } = 2f;
    [field: SerializeField] public float MoveSpeed { get; set; } = 3.25f;

    [SerializeField] public GameObject Projectile;

    #region State Machine Variables
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState EnemyIdleState { get; set; }
    public EnemyChaseState EnemyChaseState { get; set; }
    public EnemyAttackState EnemyAttackState { get; set; }
    
    #endregion

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        EnemyIdleState = new EnemyIdleState(this, StateMachine);
        EnemyChaseState = new EnemyChaseState(this, StateMachine);
        EnemyAttackState = new EnemyAttackState(this, StateMachine);
    }
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        CurrentHealth = MaxHealth;
        StateMachine.Initialize(EnemyChaseState);
        
    }

    private void Update()
    {
        Agent.destination = TargetDestination.position;
        StateMachine.CurrentEnemyState.FrameUpdate();
        Agent.speed = MoveSpeed;
    }

    #region Health / Die function
    public void Damage(float damageAmount)
        {
            CurrentHealth -= damageAmount;

            if(CurrentHealth <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    #endregion

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    #region Animation Triggers
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound,

    }
    #endregion
}