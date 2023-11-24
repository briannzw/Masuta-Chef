using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Companion
{
    using Character;
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

        [SerializeField] protected float minDistanceFromPlayer;

        [SerializeField] private float minDistanceSlotFromPlayer;
        public CompanionStateMachine CompanionStateMachine;

        [Header("Compat Properties")]
        public float AttackDistance;
        public GameObject CurrentEnemy;
        private new void Awake()
        {
            CompanionStateMachine = new CompanionStateMachine();
            chara = GetComponent<Character>();

            Agent = GetComponent<NavMeshAgent>();
            
            CompanionStateMachine.Initialize(new CompanionIdleState(this, CompanionStateMachine));
            DetectionRadius = 8f;
        }

        private new void Start()
        {
            chara.OnDie += OnCompanionDie;
            Agent.speed = chara.Stats.StatList[StatsEnum.Speed].Value / 10;
        }

        protected void Update()
        {
            CompanionStateMachine.CurrentState.FrameUpdate();
            //if(Animator != null) Animator.SetBool("IsRunning", );



            DistanceFromPlayer = Vector3.Distance(transform.position, GameManager.Instance.PlayerTransform.position);

            NavMeshHit hit;
            Vector3 slotPosition;

            NavMesh.SamplePosition(companionSlotPosition.position, out hit, Mathf.Infinity, 1 << NavMesh.GetAreaFromName("Walkable"));
            float distanceSlot = Mathf.Abs(hit.position.y - companionSlotPosition.position.y);
            slotPosition = (distanceSlot < 0.4f) ? hit.position : FindAltPath();
            IsAltPath = (distanceSlot > 0.4f);

            SetTarget(slotPosition);

            if(IsFollowingEnemy && DistanceFromPlayer > MaxDistanceFromPlayer) IsFollowingEnemy = false;

            if(CurrentEnemy != null)
            {
                if (CurrentEnemy.GetComponent<Enemy.Enemy>().IsDead) IsFollowingEnemy = false;
            }
            
            //Debug AI
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
                        if (collider.gameObject.GetComponent<Enemy.Enemy>().IsDead) return;
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
        }

        private void OnCompanionDie()
        {
            GameManager.Instance.PlayerTransform.GetComponent<CompanionSlotManager>().DeleteCompanion(this);
            Animator.SetTrigger("Dead");
        }
    }
}
