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
        [SerializeField] GameObject engageText; //debug
        public GameObject attackText; //debug

        public float AttackDistance;
        public float AttackTimer;
        [HideInInspector]
        public float DefaultAttackTimer;
        public float AttackDuration;
        [HideInInspector]
        public float DefaultAttackDuration;

        protected new void Awake()
        {
            base.Awake();
            StateMachine = new EnemyStateMachine();
            currentTauntTimer = maxTauntTimer;
            CurrentEnemies = GameManager.Instance.PlayerTransform.gameObject;
            DefaultAttackTimer = AttackTimer;
            DefaultAttackDuration = AttackDuration;
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
            engageText.SetActive(IsEngaging);
            currentTauntTimer -= Time.deltaTime;
            if (!IsTaunted && !IsEngaging)
            {
                TargetPosition = GameManager.Instance.PlayerTransform.position;
            }

            if (IsTaunted && Agent.remainingDistance > 5f || IsTaunted && currentTauntTimer <= 0f)
            {
                RemoveTauntEffect();
            }
        }

        void EnemyDie()
        {
            killCount++;
            GameManager.Instance.OnEnemiesKilled?.Invoke();
            if (SelectedWeapon != AttackType.Joker)
            {
                GetComponent<SpawnObject>().Release();
            }
        }

        void RemoveTauntEffect()
        {
            IsTaunted = false;
            Agent.isStopped = false;
            currentTauntTimer = maxTauntTimer;
        }

         public static int GetKillCount()
        {
            return killCount;
        }
    }
}
