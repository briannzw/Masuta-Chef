using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Companion
{
    public class CompanionAttackerRanged : Companion
    {
        [SerializeField] float wanderRadius;
        [SerializeField] float wanderInterval;
        public override float WanderRadius => wanderRadius;
        public override float WanderInterval => wanderInterval;
        [SerializeField] float rotationSpeed = 5.0f;
        private new void Update()
        {
            base.Update();
            DetectTarget();

            if (followEnemy && Agent.remainingDistance <= StopDistance && DistanceFromPlayer < MaxDistanceFromPlayer)
            {
                StateMachine.ChangeState(new NPCAttackState(this, StateMachine));
            }

            if (followEnemy)
            {
                Vector3 direction = enemy.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (!followEnemy && DistanceFromPlayer <= minDistanceFromPlayer)
            {
                Agent.isStopped = false;
                shouldWander = true;
                wanderTimer -= Time.deltaTime;
                if (wanderTimer <= 0f)
                {
                    Wander();
                    wanderTimer = WanderInterval;
                }
            }
            else
            {
                shouldWander = false;
            }
        }
    }
}