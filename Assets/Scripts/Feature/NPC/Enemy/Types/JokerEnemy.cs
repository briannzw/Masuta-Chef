using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC.Enemy
{
    using Spawner;
    public class JokerEnemy : Enemy, IDetectionNPC, IWanderNPC
    {
        [SerializeField] private float pickCrateRadius = 2f;
        [field: SerializeField] public float DetectionRadius { get; set; }
        public string TargetTag { get; set; }
        [field: SerializeField] public float WanderRadius { get; set; }
        [field: SerializeField] public float WanderInterval { get; set; }
        [SerializeField] float wanderTimer = 0;
        private bool isPickingUpCrate = false;
        public LayerMask TargetMask;
        [Range(0, 360)] public float PickupAngle = 125;
        private Pickup.IPickable nearestPickable;
        [SerializeField] private Transform pickupPos;
        private bool hasCrate = false;
        private bool CanPickUp = true;
        [SerializeField] private float safeDistance = 10f;

        [SerializeField]
        private LayerMask layerMask;

        protected override void Awake()
        {
            base.Awake();
            IsThisJoker = true;
            StateMachine.Initialize(new EnemyJokerWanderState(this, StateMachine));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            StateMachine.Initialize(new EnemyJokerWanderState(this, StateMachine));
            CanPickUp = true;
            Agent.enabled = true;
        }

        private new void Start()
        {
            base.Start();
            chara.OnDie += OnJokerDie;
        }

        private new void Update()
        {
            StateMachine.CurrentState.FrameUpdate();

            if (!Agent.enabled) return;

            Animator.SetBool("IsRunning", !Agent.isStopped);
            Animator.SetBool("IsCrateRunning", hasCrate);

            DetectTarget();
            if (!isPickingUpCrate && !hasCrate)
            {
                wanderTimer -= Time.deltaTime;
                if (wanderTimer <= 0f)
                {
                    // Time to choose a new random destination.
                    Wander();
                    wanderTimer = WanderInterval;
                }
            }

            if (hasCrate)
            {
                RunAwayFromPlayer();
            }

            if (pickupPos.childCount < 1)
            {
                hasCrate = false;
            }
        }

        public void DetectTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, DetectionRadius);
            float closestDistance = Mathf.Infinity;
            Transform closestCrate = null;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Crate"))
                {
                    float distanceToCrate = Vector3.Distance(transform.position, collider.transform.position);
                    if (distanceToCrate < closestDistance)
                    {
                        closestDistance = distanceToCrate;
                        closestCrate = collider.transform;
                    }
                }
            }

            if (closestCrate != null && (!closestCrate.gameObject.GetComponent<Crate.CrateController>().IsHeld || closestCrate.gameObject.GetComponent<Crate.CrateController>().CurrentPicker.CompareTag("Crate Area")))
            {
                isPickingUpCrate = true;
                TargetPosition = closestCrate.position;
                if (Agent.remainingDistance <= StopDistance)
                {
                    PickUpCrate();
                }
            }
            else
            {
                isPickingUpCrate = false;
            }
        }

        private void PickUpCrate()
        {
            if (CanPickUp == false) return;
            if (!hasCrate)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, pickCrateRadius);
                float closestDistance = Mathf.Infinity;
                Transform nearestObject = null;

                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("Crate"))
                    {
                        float distanceToCrate = Vector3.Distance(transform.position, collider.transform.position);
                        if (distanceToCrate < closestDistance)
                        {
                            closestDistance = distanceToCrate;
                            nearestObject = collider.transform;
                        }
                    }
                }

                if (nearestObject != null)
                {
                    nearestPickable = nearestObject.GetComponent<Pickup.IPickable>();
                    hasCrate = true;
                    if (nearestPickable != null && nearestPickable.StartPickup(gameObject))
                    {
                        nearestObject.transform.parent = pickupPos;
                        nearestObject.transform.localPosition = Vector3.zero;
                        nearestObject.transform.localRotation = Quaternion.identity;
                    }
                    else
                    {
                        nearestPickable = null;
                    }
                }
            }
        }

        public void OnPickUpCancel()
        {
            if (hasCrate)
            {
                if (nearestPickable != null)
                {
                    pickupPos.DetachChildren();
                    nearestPickable.ExitPickup();
                    nearestPickable = null;
                    hasCrate = false;
                }
            }

        }

        void RunAwayFromPlayer()
        {
            // Calculate the distance between the enemy and the player
            float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.PlayerTransform.position);

            // If the player is too close, run away from the player using NavMesh
            if (distanceToPlayer < safeDistance)
            {
                // Calculate the direction away from the player
                Vector3 runDirection = transform.position - GameManager.Instance.PlayerTransform.position;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position + runDirection.normalized * safeDistance, out hit, 5f, 1 << NavMesh.GetAreaFromName("Walkable")))
                {
                    TargetPosition = hit.position;
                }
            }
            else
            {
                wanderTimer -= Time.deltaTime;
                if (wanderTimer <= 0f)
                {
                    // Time to choose a new random destination.
                    Wander();
                    wanderTimer = WanderInterval;
                }
            }
        }

        public void Wander()
        {
            Vector3 randomDirection = Random.insideUnitSphere * WanderRadius;
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position + randomDirection, out hit, WanderRadius, NavMesh.AllAreas);

            TargetPosition = hit.position;
        }

        private void OnJokerDie()
        {
            CanPickUp = false;
            OnPickUpCancel();
        }
    }
}
