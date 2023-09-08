using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RayGunController : MonoBehaviour
{
    private GameObject shootTarget;
    
    [Header("Shoot Parameter")]
    private LineRenderer rayBullet;
    public GameObject bulletOrigin;
    public float firingSpeed = 1f;
    private bool isCooldown = false;

    private void Awake() {
        rayBullet = GetComponent<LineRenderer>();
        shootTarget = transform.parent.Find("ShootTarget").gameObject;
    }

    private void OnEnable()
    {
        Shoot();
    }

    private void Update() {
        if (!isCooldown)
        {
            Shoot();
        }
    }

    #region Shoot Callback
    private void Shoot()
    {
        rayBullet.SetPosition(0, bulletOrigin.transform.position);
        rayBullet.SetPosition(1, new Vector3(0, shootTarget.transform.position.y, 10));

        StartCoroutine(ShootCooldown());
    }

    private IEnumerator ShootCooldown()
    {
        rayBullet.enabled = true;
        isCooldown = true;
        yield return new WaitForSeconds(firingSpeed);
        isCooldown = false;
        rayBullet.enabled = false;
    }
    #endregion
}
