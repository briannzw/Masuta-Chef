using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    private int floorMask;
    private float cameraRayLength = 100f;

    public GameObject gun;
    public float range = 10f;
    public float timeBetweenBullets = 0.15f;
    public float effectsDisplayTime = 0.2f;

    private Ray bulletRay;

    private LineRenderer gunLine;
    private float timer;

    // Start is called before the first frame update
    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        gunLine = gun.GetComponent<LineRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.deltaTime != 0)
        {
            Shoot();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableShootEffect();;
        }
    }

    private void DisableShootEffect()
    {
        gunLine.enabled = false;
    }

    private void Shoot()
    {
        timer = 0;

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);
        
        // bulletRay.origin = transform.position;
        // bulletRay.direction = transform.forward;
        Debug.Log("DIRECTION : " + transform.forward);

        gunLine.SetPosition(1, transform.position + transform.forward * range);
    }
    
    void FixedUpdate()
    {
        Turning();
    }

    void Turning()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit floorHit;

        if(Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            transform.rotation = newRotation;
        }
    }
}
