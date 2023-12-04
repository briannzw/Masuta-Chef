using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cooking.Gameplay;

public class AnimatorHandler : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] CookingGameplay cookingGameplay;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem missedEffect;
    [SerializeField] ParticleSystem hitEffect;
    void Start()
    {
        cookingGameplay.OnCookingHit += Hit;
        cookingGameplay.OnCookingMissed += Missed;
    }

    private void Hit()
    {
        hitEffect.Play();
        StartCoroutine(ClearParticle(hitEffect, hitEffect.main.duration));
    }

    private void Missed()
    {
        missedEffect.Play();
        StartCoroutine(ClearParticle(missedEffect, missedEffect.main.duration));
    }

    IEnumerator ClearParticle(ParticleSystem particle, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        particle.Clear();
    }
}
