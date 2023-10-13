using AYellowpaper.SerializedCollections;
using Character;
using Kryz.CharacterStats;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Weapon
{
    public class Weapon : MonoBehaviour
    {
        #region Properties
        public Character.Character holder;
        [Tag] public string TargetTag;
        public SerializedDictionary<WeaponStatsEnum, CharacterStat> stats;
        
        protected bool isFiring;
        private float attackTimer;
        #endregion

        public enum WeaponStatsEnum 
        {
            Power,
            Speed,
            Accuraccy
        }

        #region Lifecycle
        // Start is called before the first frame update
        protected void Start()
        {
            attackTimer = 0;
        }

        // Update is called once per frame
        protected void Update()
        {
            if (isFiring && attackTimer > stats[WeaponStatsEnum.Speed].Value / 100)
            {
                Attack();
                attackTimer = 0;
            }

            attackTimer += Time.deltaTime;
        }
        #endregion

        #region Method
        public virtual void Attack() 
        {
            Debug.Log("Attack Method of Weapon Not Implemented!");
        }

        public virtual void StartAttack()
        {
            isFiring = true;
        }

        public virtual void StopAttack()
        {
            isFiring = false;
        }
        #endregion
    }
}
