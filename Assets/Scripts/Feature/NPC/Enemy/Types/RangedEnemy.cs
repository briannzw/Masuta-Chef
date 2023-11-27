using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Enemy.Ranged
{
    public class RangedEnemy : Enemy
    {
        protected new void Awake()
        {
            base.Awake();
            StateMachine.Initialize(new EnemyRangedMoveState(this, StateMachine));
        }

        private void OnEnable()
        {
            StateMachine.Initialize(new EnemyRangedMoveState(this, StateMachine));
        }
    }
}
