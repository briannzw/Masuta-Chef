using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Weapon;
[RequireComponent(typeof(AudioSource))]
public class WeaponAudioController : MonoBehaviour
{
    [Header("References")]
    public AudioSource AudioSource;
    public AudioClip AttackClip;
    public Weapon.Weapon weapon;

    private void Start() 
    {
        weapon.OnAttack += Attack;
    }

    private void Attack()
    {
        AudioSource.PlayOneShot(AttackClip);
    }
}
