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
    public virtual EnemyState EnemyCombatBehaviour { get; set; }


    #region Ranged Enemy Variable
    public virtual GameObject Projectile { get; set; }
    public virtual float ProjectileSpeed { get; set; } = 14f;
    public virtual float ShootInterval { get; set; } = 1f;
    #endregion

    #region Melee Enemy Variable
    public virtual float MeleeInterval { get; set; } = 1f;
    #endregion

    #region State Machine Variables
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState EnemyIdleState { get; set; }
    public EnemyChasePlayerState EnemyChasePlayerState { get; set; }
    public EnemyShootState EnemyShootState { get; set; }
    public EnemyMeleeState EnemyMeleeState { get; set; }
    public EnemyWanderState EnemyWanderState { get; set; }

    #endregion

    protected void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        StateMachine = new EnemyStateMachine();
        EnemyIdleState = new EnemyIdleState(this, StateMachine);
        EnemyChasePlayerState = new EnemyChasePlayerState(this, StateMachine);
        EnemyShootState = new EnemyShootState(this, StateMachine);
        EnemyMeleeState = new EnemyMeleeState(this, StateMachine);
        EnemyWanderState = new EnemyWanderState(this, StateMachine);
    }
    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    protected void Update()
    {
        Agent.SetDestination(TargetDestination.position);
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
