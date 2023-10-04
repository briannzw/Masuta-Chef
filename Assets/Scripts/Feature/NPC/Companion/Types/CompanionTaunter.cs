using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionTaunter : Companion
{
    [SerializeField] float rotationSpeed = 5.0f;
    [SerializeField] int maxTauntedEnemies = 5;
    [SerializeField] float tauntRadius;
    private bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    private new void Update()
    {
        base.Update();
        DetectTarget();

        if (followEnemy && Agent.remainingDistance <= StopDistance && DistanceFromPlayer < MaxDistanceFromPlayer)
        {
            StateMachine.ChangeState(NPCAttackState);
            Taunt();
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

    void Taunt()
    {

        Collider[] colliders = Physics.OverlapSphere(GameManager.playerTransform.position, DetectionRadius);
        float closestDistance = Mathf.Infinity;
        Transform[] closestEnemies = new Transform[maxTauntedEnemies];

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(GameManager.playerTransform.position, collider.transform.position);
                for (int i = 0; i < closestEnemies.Length; i++)
                {
                    if (closestEnemies[i] == null || distanceToEnemy < closestDistance)
                    {
                        closestEnemies[i] = collider.transform;
                        closestDistance = distanceToEnemy;
                        break;
                    }
                }
            }
        }

        foreach (Transform enemy in closestEnemies)
        {
            if (enemy != null && isAlive)
            {
                enemy.GetComponent<NPC>().TargetPosition = transform.position;
                enemy.GetComponent<Enemy>().IsTaunted = true;
            }

            if (!isAlive)
            {
                enemy.GetComponent<Enemy>().IsTaunted = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tauntRadius);
    }
}