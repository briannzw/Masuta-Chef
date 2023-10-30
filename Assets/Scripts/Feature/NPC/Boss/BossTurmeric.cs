using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Boss
{
    using Character.Hit;
    using Character.StatEffect;
    public class BossTurmeric : NPC
    {
        #region Flag Condition
        private float healthCriticalPercent = 20/100;
        private bool isEnraged = false;
        private float currentTimer;
        private float skillCastMinInterval = 20f;
        private float skillCastMaxInterval = 26f;
        private float skillCastInterval;
        private bool isCastingSkill = false;
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
        private float nextVortexSpawnTime = 15f;
        [field: SerializeField] public List<Effect> RageEffects { get; set; }



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
            if (Agent.remainingDistance <= StopDistance && !isCastingSkill)
            {
                StateMachine.ChangeState(new NPCAttackState(this, StateMachine));
                RotateToTarget(rotationSpeed);
            }
            else if(!isCastingSkill)
            {
                StateMachine.ChangeState(new NPCMoveState(this, StateMachine));
            }


            //When to cast skill 1:

            if (Time.time > nextVortexSpawnTime)
            {
                CastSkill1();
                nextVortexSpawnTime += Time.time + Random.Range(15, 20);
            }

            if (isEnraged)
            {
                SwampSkill2();
            }

            if (chara.Stats.DynamicStatList[DynamicStatsEnum.Health].CurrentValue <= 0 && !isEnraged)
            {
                CastRage();
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
            Agent.isStopped = false;
            isCastingSkill = false;
        }

        void CastSkill1()
        {
            isCastingSkill = true;
            StateMachine.ChangeState(new BossCastSkillState(this, StateMachine));
            //Animator.SetTrigger("CastSkill");
            Agent.isStopped = true;
            Invoke("VortexSkill1", 3f);
        }

        void CastRage()
        {
            Debug.Log("Is Enraged");
            isCastingSkill = true;
            isEnraged = true;
            StateMachine.ChangeState(new BossCastSkillState(this, StateMachine));
            //Animator.SetTrigger("CastRage");
            Agent.isStopped = true;
            Invoke("CastingDuration", 2f);

            foreach (Effect effect in RageEffects)
            {
                effect.Initialize();
                Debug.Log("Take Effect");
                chara.AddEffect(effect);
            }
        }

        void CastingDuration()
        {
            isCastingSkill = false;
            Agent.isStopped = false;
        }

        void SwampSkill2()
        {

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
