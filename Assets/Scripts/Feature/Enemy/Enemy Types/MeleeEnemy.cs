using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee Properties")]
    [SerializeField] private float meleeAttackInterval = 1f;
    public override Transform TargetDestination => EnemySpawner.Instance.PlayerPosition;
    public override EnemyState EnemyCombatBehaviour => EnemyMeleeState;
    public override float MeleeInterval => meleeAttackInterval;

    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(EnemyChasePlayerState);
    }
}
