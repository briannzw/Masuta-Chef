using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Shoot Parameter")]
    public Transform bulletSpawnPosition;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    public float timeBetweenBullets = .2f;

    private bool isCooldown = false;
    private PlayerAction playerControls;

    private void Start()
    {
        playerControls = InputManager.PlayerAction;
        RegisterInputCallbacks();
    }

    private void OnEnable()
    {
        RegisterInputCallbacks();
    }

    private void OnDisable()
    {
        UnregisterInputCallbacks();
    }

    #region Callbacks
    private void RegisterInputCallbacks()
    {
        if(playerControls == null) return;

        playerControls.Gameplay.Fire.Enable();
        playerControls.Gameplay.Fire.performed += Shoot;
    }

    private void UnregisterInputCallbacks()
    {
        if(playerControls == null) return;

        playerControls.Gameplay.Fire.performed -= Shoot;
        playerControls.Gameplay.Fire.Disable();
    }
    #endregion

    #region Shoot Callback
    private void Shoot(InputAction.CallbackContext context)
    {
        if(isCooldown) return;

        var bullet = Instantiate(bulletPrefab, bulletSpawnPosition.position, bulletSpawnPosition.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPosition.forward * bulletSpeed;

        StartCoroutine(ShootCooldown());
    }

    private IEnumerator ShootCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(timeBetweenBullets);
        isCooldown = false;
    }
    #endregion
}
