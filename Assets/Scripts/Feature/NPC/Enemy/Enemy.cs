using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NPC.Enemy
{
    using Spawner;

    public class Enemy : NPC
    {
        
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

        [Header("Debuff Modifier")]
        public bool IsTaunted = false;
        public bool IsStun;
        public bool IsConfused;
        public bool IsDead;
        public GameObject DebuffIcon;
        public bool IsDebuffed = false;

        public GameObject ConfusedIcon;
        public GameObject StunIcon;

        protected override void OnEnable()
        {
            base.OnEnable();
            Agent.isStopped = false;
            ChildCollider.enabled = true;
            IsDead = false;
            Debug.Log("OnEnable");
        }

        protected override void Awake()
        {
            base.Awake();
            StateMachine = new EnemyStateMachine();
            currentTauntTimer = maxTauntTimer;
            CurrentEnemy = GameManager.Instance.PlayerTransform.gameObject;
            DefaultAttackTimer = AttackTimer;
            DefaultAttackDuration = AttackDuration;
            defaultEnemyTarget = GameManager.Instance.PlayerTransform.gameObject;
            ChildCollider.enabled = true;
            MaxTauntedDistance = AttackDistance;
        }

        protected void Start()
        {
            chara.OnDie += EnemyDie;
            Agent.speed += Random.Range(-0.05f, -0.3f);
        }

        protected void Update()
        {
            StateMachine.CurrentState.FrameUpdate();
            if (!IsTaunted && !IsEngaging)
            {
                TargetPosition = GameManager.Instance.PlayerTransform.position;
            }

            if (IsTaunted) //Remove taunt if outside range from taunter
            {
                RemoveTauntByDistance();
            }

            if (!IsTaunted && CurrentEnemy != defaultEnemyTarget)
            {
                CurrentEnemy = defaultEnemyTarget;
                DebuffIcon.SetActive(false);
            }

            if((IsStun || IsConfused) && !IsDebuffed && !IsDead)
            {
                IsDebuffed = true;
                StateMachine.ChangeState(new EnemyDebuffState(this, StateMachine));
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
            ChildCollider.enabled = false;

            if (GetComponent<SpawnObject>()) GetComponent<SpawnObject>().ReleaseAfter(3f);
        }

        void RemoveTauntByDistance()
        {
            if (CurrentEnemy.GetComponent<Companion.CompanionTaunter>().isAlive)
            {
                return;
            }

            IsTaunted = false;
            Agent.isStopped = false;
        }

         public static int GetKillCount()
        {
            return killCount;
        }
    }
}
