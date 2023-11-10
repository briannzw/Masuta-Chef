using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    using Character;
    using Spawner;
    using Enemy;
    public class NPC : MonoBehaviour
    {
        public enum AttackType
        {
            Melee,
            Ranged,
            Utility,
            Joker
        }
        public AttackType SelectedWeapon;
        public Weapon.Weapon ActiveWeapon;
        public Animator Animator;
        public Transform TargetTransform;
        public Vector3 TargetPosition { get; set; }
        public float StopDistance;
        [HideInInspector]
        public NavMeshAgent Agent;
        //public Character Character;
        public NPCStateMachine StateMachine;
        protected Character chara;

        #region Joker Properties
        [HideInInspector]
        public bool IsThisJoker = false;
        #endregion

        protected void Awake()
        {
            StateMachine = new NPCStateMachine();
            chara = GetComponent<Character>();

            Agent = GetComponent<NavMeshAgent>();

            Agent.stoppingDistance = StopDistance;


        }
        // Start is called before the first frame update
        protected void Start()
        {
            if (ActiveWeapon != null) ActiveWeapon.OnEquip(chara);
            Agent.speed = chara.Stats.StatList[StatsEnum.Speed].Value / 10;
            chara.OnSpeedChanged += () => Agent.speed = chara.Stats.StatList[StatsEnum.Speed].Value / 10;
        }

        // Update is called once per frame
        protected void Update()
        {
            StateMachine.CurrentState.FrameUpdate();
        }
    }
}

