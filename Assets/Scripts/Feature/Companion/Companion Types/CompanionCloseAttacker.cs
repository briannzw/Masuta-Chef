using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCloseAttacker : Companion
{
    [SerializeField] private float detectEnemyRadius = 10f;
    public override Transform TargetDestination => EnemySpawner.Instance.PlayerPosition;
    public override float DetectEnemyRadius => detectEnemyRadius;

    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(CompanionChasePlayerState);
    }

    private new void Update()
    {
        StateMachine.CurrentCompanionState.FrameUpdate();
        if (MaxDistanceTowardsPlayer > 11)
        {
            StateMachine.ChangeState(CompanionChasePlayerState);
        }
        if(Agent.remainingDistance <= MaxDistanceTowardsPlayer)
        {
            StateMachine.ChangeState(CompanionIdleState);
        }
        Agent.speed = MoveSpeed;
    }
}
