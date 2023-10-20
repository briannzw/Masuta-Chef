using Character.Hit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExplod : MonoBehaviour
{
    #region Properties
    [Header("Explosion Components")]
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private AOEController controller;
    [SerializeField] private Collider collider;

    private float sphereRadius;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        explosionParticle.Play();
        Destroy(explosionParticle, controller.AreaDuration);

        sphereRadius = (collider as SphereCollider).radius;
        //(collider as SphereCollider).radius = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        //if((collider as SphereCollider).radius < sphereRadius)
        //{
        //    (collider as SphereCollider).radius += Time.deltaTime * (sphereRadius - (collider as SphereCollider).radius);
        //}
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, (collider as SphereCollider).radius);
    }
#endif
}
