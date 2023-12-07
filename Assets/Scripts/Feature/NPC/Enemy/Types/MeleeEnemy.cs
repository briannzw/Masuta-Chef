using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Enemy.Melee
{
    public class MeleeEnemy : Enemy
    {
        protected override void Awake()
        {
            base.Awake();
            StateMachine.Initialize(new EnemyMeleeMoveState(this, StateMachine));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            StateMachine.Initialize(new EnemyMeleeMoveState(this, StateMachine));
        }
    }
}

