using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cooking.Gameplay;
[RequireComponent(typeof(AudioSource))]
public class CookingAudioController : MonoBehaviour
{
    [Header("References")]
    public AudioSource AudioSource;
    public AudioClip SuccessClip;
    public AudioClip FailedClip;
    public AudioClip HitClip;
    public AudioClip MissedClip;
    public CookingGameplay cookingGameplay;

    private void Start()
    {
        cookingGameplay.OnCookingSuccess += Success;
        cookingGameplay.OnCookingFailed += Failed;
        cookingGameplay.OnCookingHit += Hit;
        cookingGameplay.OnCookingMissed += Missed;
    }

    private void Success()
    {
        AudioSource.PlayOneShot(SuccessClip);
    }

    private void Failed()
    {
        AudioSource.PlayOneShot(FailedClip);
    }

    private void Hit()
    {
        AudioSource.PlayOneShot(HitClip);
    }

    private void Missed()
    {
        AudioSource.PlayOneShot(MissedClip);
    }

}
