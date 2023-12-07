using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Companion
{
    using Character;
    using Enemy;
    public class CompanionTaunter : Companion
    {
        [SerializeField] float rotationSpeed = 5.0f;
        [SerializeField] int maxTauntedEnemies = 5;
        [SerializeField] float tauntRadius;
        public bool isAlive = true;

        protected override void OnEnable()
        {
            CompanionStateMachine.Initialize(new GourdakinIdleState(this, CompanionStateMachine));
            isAlive = true;
        }

        private void OnDisable()
        {
            isAlive = false;
        }
        protected override void Awake()
        {
            CompanionStateMachine = new CompanionStateMachine();
            
            chara = GetComponent<Character>();

            Agent = GetComponent<NavMeshAgent>();
            CompanionStateMachine.Initialize(new GourdakinIdleState(this, CompanionStateMachine));
        }

        protected override void Start()
        {
            base.Start();
            chara.OnDie += OnTaunterDie;
        }


        protected override void Update()
        {
            base.Update();
            if (isAlive)
            {
                Taunt();
            }
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, tauntRadius);
        }

        void Taunt()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, tauntRadius);
            List<Transform> closestEnemies = new List<Transform>();

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy") && collider.GetComponent<NPC>().SelectedWeapon != AttackType.Joker)
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
                            closestEnemies[closestEnemies.Count - 1].GetComponent<Enemy>().IsTaunted = false;
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
                    enemy.GetComponent<Enemy>().DebuffIcon.SetActive(true);
                    enemy.GetComponent<NPC>().CurrentEnemy = gameObject;
                }
            }
        }

        void OnTaunterDie()
        {
            isAlive = false;
        }
    }
}