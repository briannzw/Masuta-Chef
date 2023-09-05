using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemyMoveable
{
    float MoveSpeed { get; set; }
    Transform TargetDestination { get; set; }
    NavMeshAgent Agent { get; set; }
    float MaxDistanceTowardsPlayer { get; set; }
}
