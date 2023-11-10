using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Hit;
using Module.Detector;
using Character;

public class CreamExplosion : AOEController
{
    #region Properties
    [Header("Explosion Components")]
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float explosionRadius;
    #endregion

    protected override void Start()
    {
        base.Start();
        explosionParticle.Play();
        // Destroy(explosionParticle, controller.AreaDuration);
        Initialize(Source);
        Attack();
    }

    private void Attack()
    {
        List<Character.Character> affectedCharacter = ColliderDetector.Find<Character.Character>(transform.position, radius: explosionRadius, layerMask, transform.forward);
        if(affectedCharacter.Count > 0)
        {
            foreach (var chara in affectedCharacter)
            {
                HitChara(chara);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
#endif
}
