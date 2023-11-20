using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimationAttack : MonoBehaviour
{
    public Weapon.Weapon ActiveWeapon;

    public void StartAttack()
    {
        ActiveWeapon.StartAttack();
    }

    public void StopAttack()
    {
        ActiveWeapon.StopAttack();
    }
}
