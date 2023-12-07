using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Enemy.Ranged
{
    public class RangedEnemy : Enemy
    {
        protected override void Awake()
        {
            base.Awake();
            StateMachine.Initialize(new EnemyRangedMoveState(this, StateMachine));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            StateMachine.Initialize(new EnemyRangedMoveState(this, StateMachine));
        }
    }
}
