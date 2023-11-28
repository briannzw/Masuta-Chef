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

        protected Character chara;

        [Header("Combat Properties")]
        public bool IsEngaging = false;
        public GameObject CurrentEnemy;

        #region Joker Properties
        [HideInInspector]
        public bool IsThisJoker = false;
        #endregion

        protected void Awake()
        {
            chara = GetComponent<Character>();

            Agent = GetComponent<NavMeshAgent>();

            Agent.stoppingDistance = StopDistance;
        }

        private void OnEnable()
        {
            if (ActiveWeapon != null) ActiveWeapon.OnEquip(chara);
            Agent.speed = chara.Stats.StatList[StatsEnum.Speed].Value / 10;
            chara.OnSpeedChanged += () => Agent.speed = chara.Stats.StatList[StatsEnum.Speed].Value / 10;
        }
    }
}

