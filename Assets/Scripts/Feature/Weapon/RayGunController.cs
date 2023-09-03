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
    public float timeBetweenBullets = 1f;
    private bool isCooldown = false;
    private PlayerAction playerControls;

    void Start()
    {
        Transform parentTransform = transform.parent;
        shootTarget = parentTransform.Find("ShootTarget").gameObject;

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
        StartCoroutine(ShootCooldown());

        rayBullet.SetPosition(0, bulletOrigin.transform.position);
        rayBullet.SetPosition(1, new Vector3(0, shootTarget.transform.position.y, 10));
    }

    private IEnumerator ShootCooldown()
    {
        rayBullet.enabled = true;
        isCooldown = true;
        yield return new WaitForSeconds(timeBetweenBullets);
        isCooldown = false;
        rayBullet.enabled = false;
    }
    #endregion
}
