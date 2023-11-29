using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Companion
{
    using Character;
    using Data;
    using Player.CompanionSlot;
    public class Companion : NPC, IDetectionNPC
    {
        public float MaxDistanceFromPlayer;
        public bool IsAltPath;

        public virtual float WanderRadius { get; set; }
        public virtual float WanderInterval { get; set; }
        public float DetectionRadius { get; set; }
        public string TargetTag { get; set; }
        public bool IsFollowingEnemy = false;
        [HideInInspector]
        public float DistanceFromPlayer;
        protected Transform enemy;
        public Transform companionSlotPosition;
        public int companionSpawnOrder;

        private Vector3 playerPos;

        [SerializeField] protected float minDistanceFromPlayer;

        [SerializeField] private float minDistanceSlotFromPlayer;
        public CompanionStateMachine CompanionStateMachine;

        [Header("Compat Properties")]
        public float AttackDistance;

        private void OnEnable()
        {
            Agent.isStopped = false;
            ChildCollider.enabled = true;
        }

        [Header("Data")]
        public CompanionData data;

        private new void Awake()
        {
            CompanionStateMachine = new CompanionStateMachine();
            chara = GetComponent<Character>();

            Agent = GetComponent<NavMeshAgent>();
            
            CompanionStateMachine.Initialize(new CompanionIdleState(this, CompanionStateMachine));
            DetectionRadius = 8f;
        }

        protected void Start()
        {
            chara.OnDie += OnCompanionDie;
            Agent.speed = chara.Stats.StatList[StatsEnum.Speed].Value / 10;
        }

        protected void Update()
        {
            CompanionStateMachine.CurrentState.FrameUpdate();
            playerPos = GameManager.Instance.PlayerTransform.position;
            DistanceFromPlayer = Vector3.Distance(transform.position, playerPos);

            if (!IsFollowingEnemy)
            {
                FollowPlayer();
            }

            if(IsFollowingEnemy && DistanceFromPlayer > MaxDistanceFromPlayer) IsFollowingEnemy = false;

            if(CurrentEnemy != null)
            {
                if (CurrentEnemy.GetComponent<Enemy.Enemy>().IsDead) IsFollowingEnemy = false;
            }
        }

        private void FollowPlayer()
        {
            NavMeshHit hit;
            Vector3 slotPosition;
            NavMesh.SamplePosition(companionSlotPosition.position, out hit, Mathf.Infinity, 1 << NavMesh.GetAreaFromName("Walkable"));
            float distanceSlot = Mathf.Abs(hit.position.y - companionSlotPosition.position.y);
            slotPosition = (distanceSlot < 0.4f) ? hit.position : FindAltPath();
            IsAltPath = (distanceSlot > 0.4f);

            TargetPosition = slotPosition;
        }

        private Vector3 FindAltPath()
        {
            NavMeshHit hit;
            Vector3 altPathPosition = playerPos;
            altPathPosition.x += Random.Range(-5f, 5f);
            altPathPosition.z += Random.Range(-5f, 5f);

            NavMesh.SamplePosition(altPathPosition, out hit, 4f, 1 << NavMesh.GetAreaFromName("Walkable"));
            return hit.position;
        }

        public void DetectTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(playerPos, DetectionRadius);
            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                    if (distanceToEnemy < closestDistance)
                    {
                        if (collider.gameObject.GetComponent<Enemy.Enemy>().IsDead) continue;
                        closestDistance = distanceToEnemy;
                        closestEnemy = collider.transform;
                    }
                }
            }

            if (closestEnemy != null)
            {
                IsFollowingEnemy = true;
                TargetPosition = closestEnemy.position;
                CurrentEnemy = closestEnemy.gameObject;
            }
            else
            {
                CurrentEnemy = null; // Reset CurrentEnemy if cooldown period has passed
                IsFollowingEnemy = false;
            }
        }

        private void OnCompanionDie()
        {
            Animator.SetTrigger("Dead");
            Agent.isStopped = true;
            ChildCollider.enabled = false;
        }
    }
}
