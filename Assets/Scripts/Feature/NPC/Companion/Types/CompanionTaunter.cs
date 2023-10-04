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
        Taunt();
        if (followEnemy && Agent.remainingDistance <= StopDistance && DistanceFromPlayer < MaxDistanceFromPlayer)
        {
            StateMachine.ChangeState(NPCAttackState);
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, DetectionRadius);
        List<Transform> closestEnemies = new List<Transform>();

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Transform enemyTransform = collider.transform;
                float distanceToEnemy = Vector3.Distance(transform.position, enemyTransform.position);

                // Check if enemy is closer than the existing closest enemies
                bool isCloserEnemy = closestEnemies.Count < maxTauntedEnemies || distanceToEnemy < Vector3.Distance(transform.position, closestEnemies[closestEnemies.Count - 1].position);

                if (isCloserEnemy)
                {
                    // Insert the enemy in the correct position based on distance
                    for (int i = 0; i < closestEnemies.Count; i++)
                    {
                        if (distanceToEnemy < Vector3.Distance(transform.position, closestEnemies[i].position))
                        {
                            closestEnemies.Insert(i, enemyTransform);
                            break;
                        }
                    }

                    // If the list is not full yet, add the enemy to the end
                    if (closestEnemies.Count < maxTauntedEnemies)
                    {
                        closestEnemies.Add(enemyTransform);
                    }

                    // Ensure the list does not exceed maxTauntedEnemies
                    if (closestEnemies.Count > maxTauntedEnemies)
                    {
                        closestEnemies.RemoveAt(closestEnemies.Count - 1);
                    }
                }
            }
        }

        // Set taunt properties for the closest enemies
        foreach (Transform enemy in closestEnemies)
        {
            if (enemy != null && isAlive)
            {
                enemy.GetComponent<Enemy>().IsTaunted = true;
                enemy.GetComponent<NPC>().TargetPosition = transform.position;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tauntRadius);
    }
}