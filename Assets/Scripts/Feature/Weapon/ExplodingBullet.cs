using Character.Hit;
using UnityEngine;
using Spawner;

public class ExplodingBullet : Bullet
{
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float damageScaling;

    [Header("Explosion Properties")]
    public GameObject explosionApplicator;
    // Start is called before the first frame update

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= Mathf.Abs(TravelDistance))
        {
            OnHit();
            GetComponent<SpawnObject>().Release();
        }
    }

    protected void OnHit()
    {
        GameObject explosionController = Instantiate(explosionApplicator, transform.position, transform.rotation);
        var hitController = explosionController.GetComponent<HitController>();
        hitController.Initialize(GetComponent<BulletHit>().Source, damageScaling);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Play Some Effect
        OnHit();
        GetComponent<SpawnObject>().Release();
    }
}
