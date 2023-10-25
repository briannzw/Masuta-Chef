using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Boss
{
    using Character.Hit;
    public class BossTurmeric : NPC
    {
        #region Flag Condition
        private float healthCriticalPercent = 20/100;
        private bool isEnraged = false;
        private float currentTimer;
        private float skillCastMinInterval = 20f;
        private float skillCastMaxInterval = 26f;
        private float skillCastInterval;
        #endregion

        private float currentTauntTimer;
        [SerializeField] float skillCastingDuration = 5f;
        [SerializeField] float rotationSpeed = 5.0f;

        [Header("Skill 1 Properties")]
        [SerializeField] GameObject vortexPrefab;
        [SerializeField] float vortexRadius = 5f;
        [SerializeField] float vortexSpeed = 4.5f;
        [SerializeField] float spawnAreaVortex = 20f;

        private float vortexDuration = 15f; 
        private int minVortex = 7;         // Minimum number of vortex
        private int maxVortex = 12;         // Maximum number of vortex

        [Header("Skill 2 Properties")]
        [SerializeField] float swampDuration = 10f;
        [SerializeField] float swampRadius = 5f;
        [SerializeField] GameObject explosionPrefab;
        [SerializeField] float explosionLifetime = 2f;

        private bool canCastSkill2 = false;
        private float explosionInterval = 2f;  
        private int minExplosions = 6;         // Minimum number of explosions
        private int maxExplosions = 10;         // Maximum number of explosions

        private float nextExplosionTime = 0f;
        private float nextVortexSpawnTime = 0f;

        

        private new void Awake()
        {
            base.Awake();
            StateMachine.Initialize(new NPCMoveState(this, StateMachine));
        }

        void Start()
        {
            skillCastInterval = Random.Range(skillCastMinInterval, skillCastMaxInterval);
        }
        
        protected new void Update()
        {
            base.Update();
            TargetPosition = GameManager.Instance.PlayerTransform.position;
            if (Agent.remainingDistance <= StopDistance)
            {
                Agent.isStopped = true;
                StateMachine.ChangeState(new NPCAttackState(this, StateMachine));
                RotateToTarget(rotationSpeed);
            }
            else
            {
                Agent.isStopped = false;
                StateMachine.ChangeState(new NPCMoveState(this, StateMachine));
            }

            if (Time.time > nextVortexSpawnTime)
            {
                VortexSkill1();
                nextVortexSpawnTime = Time.time + vortexDuration;
            }

            SwampSkill2();

            skillCastInterval -= Time.deltaTime;

            if(skillCastInterval <= 0)
            {
                //cast skill;
            }

            if (chara.Stats.DynamicStatList[DynamicStatsEnum.Health].CurrentValue <= healthCriticalPercent * chara.Stats.DynamicStatList[DynamicStatsEnum.Health].Value)
            {
                isEnraged = true;
            }

            /* TO DO: (if isEnraged)
             * Play Start Enraged Animation
             * Change state to casting skill 2 (charge by substracting skillCastingDuration variable to 0)
             * if(canCast)
             * {
             *      play casting animation();
             *      call SwampSkill2();\
             *      canCast = false;
             * }
             * else
             * {
             *      play casting failed animation();
             *      canCast = false;
             * }
             * 
             */
        }

        void SwampSkill2()
        {
            //if (!isEnraged) return;

            Collider[] colliders = Physics.OverlapSphere(transform.position, swampRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {


                    // Do DOT and apply slow debuff to Player
                }
            }

            if (Time.time > nextExplosionTime)
            {
                int numberOfExplosions = Random.Range(minExplosions, maxExplosions + 1);

                for (int i = 0; i < numberOfExplosions; i++)
                {
                    Vector3 randomExplosionPosition = Random.insideUnitSphere * swampRadius;

                    NavMeshHit hit;
                    NavMesh.SamplePosition(transform.position + randomExplosionPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);
                    randomExplosionPosition.y = 0.5f;
                    randomExplosionPosition = hit.position;
                    GameObject explosion = Instantiate(explosionPrefab, randomExplosionPosition, Quaternion.identity);
                    explosion.GetComponent<HitController>().Initialize(ActiveWeapon);
                    Destroy(explosion, explosionLifetime);
                }

                nextExplosionTime = Time.time + explosionInterval;
            }
        }

        void VortexSkill1()
        {
            int numberOfExplosions = Random.Range(minVortex, maxVortex);

            for (int i = 0; i < numberOfExplosions; i++)
            {
                Vector3 randomVortexPosition = Random.insideUnitSphere * spawnAreaVortex;

                NavMeshHit hit;
                NavMesh.SamplePosition(transform.position + randomVortexPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);
                randomVortexPosition.y = 0.5f;
                randomVortexPosition = hit.position;
                GameObject vortex = Instantiate(vortexPrefab, randomVortexPosition, Quaternion.identity);
                vortex.GetComponent<HitController>().Initialize(ActiveWeapon);
                Destroy(vortex, vortexDuration);
            }
        }

        protected void RotateToTarget(float rotationSpeed)
        {
            // Calculate the direction from this GameObject to the target
            Vector3 direction = TargetPosition - transform.position;
            direction.y = 0;

            // Create a rotation that looks in the calculated direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
