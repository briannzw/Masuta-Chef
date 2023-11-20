using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimationAttack : MonoBehaviour
{
    public Weapon.Weapon ActiveWeapon;

    public void Attack()
    {
        ActiveWeapon.Attack();
    }
}
