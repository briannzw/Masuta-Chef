using NPC.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedEngageState : EnemyState
{
    private Transform centerObject; // The object to circle around
    private float radius = 6f; // The radius of the circle
    Vector3 enemyPos;

    private float angle; // Current angle of rotation
    private float randomSpeed;
    private bool isClockwise; // Variable to determine the rotation direction
    public EnemyRangedEngageState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Agent.stoppingDistance = 0;
        enemy.Agent.isStopped = false;
        enemy.IsEngaging = true;
        isClockwise = Random.Range(0, 2) == 0;
        randomSpeed = Random.Range(0.4f, 0.7f) * (isClockwise ? 1 : -1); ;
        radius = Random.Range(4f, 8.5f);

        FindClosestAngle();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.IsEngaging = false;
        enemy.AttackTimer = enemy.DefaultAttackTimer;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.AttackTimer -= Time.deltaTime;
        centerObject = GameManager.Instance.PlayerTransform;
        enemyPos = enemy.transform.position;
        angle += randomSpeed * Time.deltaTime;

        // Update the angle based on a random speed


        // Calculate the position on the circle using polar coordinates
        float x = centerObject.position.x + Mathf.Cos(angle) * radius;
        float z = centerObject.position.z + Mathf.Sin(angle) * radius;

        NavMeshHit hit;
        NavMesh.SamplePosition(new Vector3(x, enemy.transform.position.y, z), out hit, Mathf.Infinity, NavMesh.AllAreas);

        enemy.Agent.SetDestination(hit.position);

        if (Vector3.SqrMagnitude(centerObject.position - enemyPos) > enemy.CombatEngageDistance)
        {
            enemy.StateMachine.ChangeState(new EnemyRangedMoveState(enemy, enemy.StateMachine));
        }

        RotateToTarget(15f);

        if (enemy.AttackTimer <= 0)
        {
            enemy.StateMachine.ChangeState(new EnemyRangedAttackState(enemy, enemy.StateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void RotateToTarget(float rotationSpeed)
    {
        // Calculate the direction from this GameObject to the target
        Vector3 direction = enemy.CurrentEnemy.transform.position - enemyPos;
        direction.y = 0;

        // Create a rotation that looks in the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate towards the target rotation
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private float FindClosestAngle()
    {
        Vector3 currentPosition = enemy.transform.position;

        float angleToCenter = Mathf.Atan2(currentPosition.z - enemy.CurrentEnemy.transform.position.z, currentPosition.x - enemy.CurrentEnemy.transform.position.x) * Mathf.Rad2Deg;

        angleToCenter = (angleToCenter + 360) % 360;

        float closestAngle = Mathf.Floor(angleToCenter / 90) * 90;
        angle = Random.Range(closestAngle, closestAngle + 90);

        return angle;
    }
}
