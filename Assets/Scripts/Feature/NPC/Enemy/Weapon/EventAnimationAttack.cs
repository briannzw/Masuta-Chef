using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawner;
using Player.CompanionSlot;
using NPC.Companion;
public class EventAnimationAttack : MonoBehaviour
{
    public Weapon.Weapon ActiveWeapon;
    public NPC.NPC NPC;

    public void Attack()
    {
        ActiveWeapon.Attack();
    }

    public void Dead()
    {
        if (NPC.gameObject.CompareTag("Companion"))
        {
            GameManager.Instance.PlayerTransform.GetComponent<CompanionSlotManager>().DeleteCompanion(NPC.GetComponent<Companion>());
            return;
        }
    }

    public void StopDamage()
    {
        ActiveWeapon.weaponCollider.enabled = false;
    }
}
