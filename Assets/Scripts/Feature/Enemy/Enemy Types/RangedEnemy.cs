using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public override Transform TargetDestination
    {
        get { return EnemySpawner.Instance.PlayerPosition; }
        set { }
    }

}
