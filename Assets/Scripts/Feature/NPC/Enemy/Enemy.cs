using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NPC.Enemy
{
    using Spawner;

    public class Enemy : NPC
    {
        public bool IsTaunted = false;
        private float currentTauntTimer;
        [SerializeField] private float maxTauntTimer = 0.1f;
        public float rotationSpeed = 5.0f;
        private static int killCount = 0;
        public EnemyStateMachine StateMachine;

        [Header("Combat Properties")]

        public float AttackDistance;
        public float CombatEngageDistance = 7f;
        public float AttackTimer;
        [HideInInspector]
        public float DefaultAttackTimer;
        public float AttackDuration;
        [HideInInspector]
        public float DefaultAttackDuration;
        [HideInInspector]
        public float MaxTauntedDistance;
        [HideInInspector]
        private GameObject defaultEnemyTarget;
        public bool IsStun;
        public bool IsConfused;
        public bool IsDead;

        public Collider childCollider;

        private void OnEnable()
        {
            Agent.isStopped = false;
            childCollider.enabled = true;
            IsDead = false;
        }

        protected new void Awake()
        {
            base.Awake();
            StateMachine = new EnemyStateMachine();
            currentTauntTimer = maxTauntTimer;
            CurrentEnemies = GameManager.Instance.PlayerTransform.gameObject;
            DefaultAttackTimer = AttackTimer;
            DefaultAttackDuration = AttackDuration;
            defaultEnemyTarget = GameManager.Instance.PlayerTransform.gameObject;
            childCollider.enabled = true;
        }

        protected new void Start()
        {
            base.Start();
            chara.OnDie += EnemyDie;
            Agent.speed += Random.Range(-0.05f, -0.3f);
        }

        protected void Update()
        {
            StateMachine.CurrentState.FrameUpdate();
            currentTauntTimer -= Time.deltaTime;
            if (!IsTaunted && !IsEngaging)
            {
                TargetPosition = GameManager.Instance.PlayerTransform.position;
            }

            if (IsTaunted && currentTauntTimer <= 0f)
            {
                RemoveTauntEffect();
            }
        }

        void EnemyDie()
        {
            killCount++;
            Agent.isStopped = true;
            //to do change to dead state
            IsDead = true;
            if (Animator != null)
            {
                Animator.SetTrigger("Dead");
                StateMachine.ChangeState(new EnemyDeadState(this, StateMachine));
            }
            GameManager.Instance.OnEnemiesKilled?.Invoke();
            childCollider.enabled = false;

        }

        void RemoveTauntEffect()
        {
            if (Vector3.Distance(CurrentEnemies.transform.position, transform.position) < MaxTauntedDistance)
            {
                return;
            }

            IsTaunted = false;
            Agent.isStopped = false;
            currentTauntTimer = maxTauntTimer;
            CurrentEnemies = defaultEnemyTarget;
        }

         public static int GetKillCount()
        {
            return killCount;
        }
    }
}
