using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAttackerClose : Companion
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

        if (followEnemy)
        {
            // Calculate the direction from this GameObject to the target
            Vector3 direction = enemy.position - transform.position;

            // Create a rotation that looks in the calculated direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (followEnemy && Agent.remainingDistance <= StopDistance && DistanceFromPlayer < MaxDistanceFromPlayer)
        {
            StateMachine.ChangeState(new NPCAttackState(this, StateMachine));
        }
    }

    private void OnDrawGizmos()
    {
        if(GameManager.Instance.PlayerTransform.position != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(GameManager.Instance.PlayerTransform.position, DetectionRadius);
        }
    }
}
