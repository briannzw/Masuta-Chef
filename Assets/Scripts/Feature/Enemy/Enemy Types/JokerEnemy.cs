using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerEnemy : Enemy
{
    public override Transform TargetDestination => EnemySpawner.Instance.PlayerPosition;
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(EnemyWanderState);
    }
}
