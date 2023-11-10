using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Companion
{
    using Character;
    public class Companion : NPC, IDetectionNPC
    {
        public float MaxDistanceFromPlayer;

        public virtual float WanderRadius { get; set; }
        public virtual float WanderInterval { get; set; }
        public float DetectionRadius { get; set; }
        public string TargetTag { get; set; }
        public bool IsFollowingEnemy = false;
        [HideInInspector]
        public float DistanceFromPlayer;
        protected Transform enemy;
        public Transform companionSlotPosition;
        [SerializeField] protected float minDistanceFromPlayer;

        [SerializeField] private float minDistanceSlotFromPlayer;
        public CompanionStateMachine CompanionStateMachine;
        private new void Awake()
        {
            CompanionStateMachine = new CompanionStateMachine();
            chara = GetComponent<Character>();

            Agent = GetComponent<NavMeshAgent>();
            
            CompanionStateMachine.Initialize(new CompanionMoveState(this, CompanionStateMachine));
            DetectionRadius = 8f;
        }

        private new void Start()
        {
            chara.OnDie += OnCompanionDie;
            Agent.speed = chara.Stats.StatList[StatsEnum.Speed].Value / 10;
        }

        protected new void Update()
        {
            CompanionStateMachine.CurrentState.FrameUpdate();
            if(Animator != null) Animator.SetBool("IsRunning", Agent.remainingDistance > 2.5);



            DistanceFromPlayer = Vector3.Distance(transform.position, GameManager.Instance.PlayerTransform.position);

            NavMeshHit hit;
            Vector3 slotPosition;

            NavMesh.SamplePosition(companionSlotPosition.position, out hit, Mathf.Infinity, 1 << NavMesh.GetAreaFromName("Walkable"));
            float distanceSlot = Mathf.Abs(hit.position.y - companionSlotPosition.position.y);
            slotPosition = (distanceSlot < 0.4f) ? hit.position : FindAltPath();

            SetTarget(slotPosition);

            if(IsFollowingEnemy && DistanceFromPlayer > MaxDistanceFromPlayer) IsFollowingEnemy = false;

            if (DistanceFromPlayer < 4f && !IsFollowingEnemy) Agent.stoppingDistance = 4f;
            else Agent.stoppingDistance = 2f;
        }

        private void SetTarget(Vector3 targetPos)
        {
            if (IsFollowingEnemy) return;

            TargetPosition = targetPos;
        }

        private Vector3 FindAltPath()
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(GameManager.Instance.PlayerTransform.position, out hit, 4f, 1 << NavMesh.GetAreaFromName("Walkable"));
            return hit.position;
        }

        public void DetectTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(GameManager.Instance.PlayerTransform.position, DetectionRadius);
            float closestDistance = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = collider.transform;
                    }
                }
            }

            if (closestEnemy != null)
            {
                IsFollowingEnemy = true;
                TargetPosition = closestEnemy.position;
            }
        }

        private void OnCompanionDie()
        {
            Destroy(gameObject);
        }
    }
}
