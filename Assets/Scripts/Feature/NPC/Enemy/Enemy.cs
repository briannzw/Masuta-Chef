using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        

        [SerializeField] GameObject engageText;

        protected new void Awake()
        {
            base.Awake();
            StateMachine.Initialize(new EnemyMoveState(this, StateMachine));
            currentTauntTimer = maxTauntTimer;
            CurrentEnemies = GameManager.Instance.PlayerTransform.gameObject;
        }

        protected new void Start()
        {
            base.Start();
            chara.OnDie += EnemyDie;
            Agent.speed += Random.Range(-0.05f, -0.3f);
        }

        protected new void Update()
        {
            base.Update();
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Agent.destination, 2f);
        }
    }
}
