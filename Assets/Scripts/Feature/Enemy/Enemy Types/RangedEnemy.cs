using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Shooting Properties")]

    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private GameObject projectileObj;

    public override Transform TargetDestination => EnemySpawner.Instance.PlayerPosition;
    public override float ProjectileSpeed => projectileSpeed;
    public override float ShootInterval => shootInterval;
    public override GameObject Projectile => projectileObj;


}
