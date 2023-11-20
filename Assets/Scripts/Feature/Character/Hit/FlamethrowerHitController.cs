using System.Collections;
using System.Collections.Generic;
using Character.Dummy;
using Character.Hit;
using MyBox;
using UnityEngine;

namespace Character.Hit
{
    using StatEffect;
    public class FlamethrowerAOEController : HitController
    {
        #region Properties
        private bool canFire = true;

        // Get reference from player and weapon
        private CreamyDispenserController weapon; 
        #endregion

        #region Collision Controller    
        private void OnTriggerStay(Collider other) 
        {
            if (canFire)
            {
                if (!Source.TargetTags.Contains(other.tag)) return;

                Character character = other.GetComponent<Character>();
                if(character == null) return;

                Hit(character);
                Debug.Log("Hitting Enemy : "+other.name);
                StartCoroutine(Cooldown());
            }
        }
        #endregion

        #region Helper Function
        private IEnumerator Cooldown()
        {
            canFire = false;
            yield return new WaitForSeconds(Source.stats[Weapon.WeaponStatsEnum.Speed].Value / 100);
            canFire = true;
        }   
        #endregion
    }
}

