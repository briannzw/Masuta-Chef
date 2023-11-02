using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Boss
{
    public class VortexController : MonoBehaviour
    {
        [SerializeField] float wanderRadius;
        [SerializeField] float wanderInterval;
        [SerializeField] float moveSpeed;

        private float wanderTimer;
        private NavMeshAgent agent;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            agent.speed = moveSpeed;
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f)
            {
                Wander();
                wanderTimer = wanderInterval;
            }
        }

        public void Wander()
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position + randomDirection, out hit, wanderRadius, NavMesh.AllAreas);

            agent.SetDestination(hit.position);
        }
    }
}

