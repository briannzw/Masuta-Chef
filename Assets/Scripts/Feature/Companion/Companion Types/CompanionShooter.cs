using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionShooter : Companion
{


    [Header("Shooting Properties")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed = 14f;
    [SerializeField] private float shootInterval = 1f;
    public override GameObject Projectile => projectile;
    public override float ProjectileSpeed => projectileSpeed;
    public override float ShootInterval => shootInterval;

    public override CompanionState CompanionCombatBehaviour => CompanionShootState;
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(CompanionChasePlayerState);
    }

    private new void Update()
    {
        base.Update();
    }
}
