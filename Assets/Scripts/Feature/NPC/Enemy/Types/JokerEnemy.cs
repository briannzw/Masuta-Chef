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

        private new void Awake()
        {
            base.Awake();
            IsThisJoker = true;
            StateMachine.Initialize(new EnemyJokerWanderState(this, StateMachine));
        }

        private new void Start()
        {
            base.Start();
            chara.OnDie += OnJokerDie;
        }

        private new void Update()
        {
            StateMachine.CurrentState.FrameUpdate();

            // Testing Drop Crate
            if (Input.GetKeyDown(KeyCode.B))
            {
                if(hasCrate) OnPickUpCancel();

            }

            Animator.SetBool("IsRunning", !Agent.isStopped);
            Animator.SetBool("IsCrateRunning", hasCrate);
            if (Agent.remainingDistance <= StopDistance)
            {
                Agent.isStopped = true;
            }
            else
            {
                Agent.isStopped = false;
            }

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
                NewFleeAI();
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

                // Calculate the target position by adding the run direction to the enemy's position
                TargetPosition = transform.position + runDirection.normalized * safeDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(TargetPosition, out hit, 5f, 1 << NavMesh.GetAreaFromName("Walkable")))
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

        private void NewFleeAI()
        {
            if(Vector3.Distance(transform.position, GameManager.Instance.PlayerTransform.position) < safeDistance)
            {
                //We will check if enemy can flee to the direction opposite from the player, we will check if there are obstacles
                bool isDirSafe = false;

                //We will need to rotate the direction away from the player if straight to the opposite of the player is a wall
                float vRotation = 0;

                while (!isDirSafe)
                {
                    //Calculate the vector pointing from Player to the Enemy
                    Vector3 dirToPlayer = transform.position - GameManager.Instance.PlayerTransform.position;

                    //Calculate the vector from the Enemy to the direction away from the Player the new point
                    Vector3 newPos = transform.position + dirToPlayer;

                    //Rotate the direction of the Enemy to move
                    newPos = Quaternion.Euler(0, vRotation, 0) * newPos;

                    //Shoot a Raycast out to the new direction with 5f length (as example raycast length) and see if it hits an obstacle
                    bool isHit = Physics.Raycast(transform.position, newPos, out RaycastHit hit, 3f, layerMask);

                    if (hit.transform == null)
                    {
                        //If the Raycast to the flee direction doesn't hit a wall then the Enemy is good to go to this direction
                        Agent.SetDestination(newPos);
                        isDirSafe = true;
                    }

                    //Change the direction of fleeing is it hits a wall by 20 degrees
                    if (isHit && hit.transform.CompareTag("Wall"))
                    {
                        vRotation += 20;
                        isDirSafe = false;
                    }
                    else
                    {
                        //If the Raycast to the flee direction doesn't hit a wall then the Enemy is good to go to this direction
                        Agent.SetDestination(newPos);
                        isDirSafe = true;
                    }
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


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Agent.destination, DetectionRadius);
        }
        private void OnJokerDie()
        {
            CanPickUp = false;
            OnPickUpCancel();
            Animator.SetTrigger("Dead");
            Invoke("ReleaseObject", 1f);
        }

        private void ReleaseObject()
        {
            GetComponentInChildren<SpawnObject>().Release();
        }
    }
}
