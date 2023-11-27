using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawner;
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
        NPC.GetComponent<SpawnObject>().Release();
    }

    public void StopDamage()
    {
        ActiveWeapon.weaponCollider.enabled = false;
    }
}
