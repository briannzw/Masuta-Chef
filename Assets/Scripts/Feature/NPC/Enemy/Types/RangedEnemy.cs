using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Enemy.Ranged
{
    public class RangedEnemy : Enemy
    {
        private new void Awake()
        {
            base.Awake();
            StateMachine.Initialize(new EnemyRangedMoveState(this, StateMachine));
        }

        private new void OnEnable()
        {
            base.OnEnable();
            StateMachine.Initialize(new EnemyRangedMoveState(this, StateMachine));
        }
    }
}
