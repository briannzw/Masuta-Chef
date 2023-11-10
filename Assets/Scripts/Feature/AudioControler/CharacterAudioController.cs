using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Character;
[RequireComponent(typeof(AudioSource))]
public class CharacterAudioController : MonoBehaviour
{
    [Header("References")]
    public AudioSource AudioSource;
    public AudioClip DeadClip;
    public AudioClip DamagedClip;
    public Character.Character character;

    private void Start()
    {
        character.OnDie += Die;
        character.OnDamaged += Damage;
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(DeadClip, new Vector3(5, 1, 2));
    }

    private void Damage()
    {
        AudioSource.PlayOneShot(DamagedClip);
    }
}