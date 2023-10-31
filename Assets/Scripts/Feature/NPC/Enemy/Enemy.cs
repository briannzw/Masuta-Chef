using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Enemy
{
    public class Enemy : NPC
    {
        public bool IsTaunted = false;
        private float currentTauntTimer;
        [SerializeField] private float maxTauntTimer = 0.1f;
        public float rotationSpeed = 5.0f;

        protected new void Awake()
        {
            base.Awake();
            StateMachine.Initialize(new EnemyMoveState(this, StateMachine));
            currentTauntTimer = maxTauntTimer;
        }

        protected new void Update()
        {
            base.Update();
            currentTauntTimer -= Time.deltaTime;
            if (!IsTaunted)
            {
                TargetPosition = GameManager.Instance.PlayerTransform.position;
            }

            if (IsTaunted && Agent.remainingDistance > 5f || IsTaunted && currentTauntTimer <= 0f)
            {
                RemoveTauntEffect();
            }
        }

        void RemoveTauntEffect()
        {
            IsTaunted = false;
            Agent.isStopped = false;
            currentTauntTimer = maxTauntTimer;
        }
    }
}
