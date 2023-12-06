using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Enemy.Melee
{
    public class MeleeEnemy : Enemy
    {
        private new void Awake()
        {
            base.Awake();
            StateMachine.Initialize(new EnemyMeleeMoveState(this, StateMachine));
        }

        private new void OnEnable()
        {
            base.OnEnable();
            StateMachine.Initialize(new EnemyMeleeMoveState(this, StateMachine));
        }
    }
}

