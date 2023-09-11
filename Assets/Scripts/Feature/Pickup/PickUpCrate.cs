using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCrate : MonoBehaviour
{

    // Pickup Crate
    [Header("PickUp")]
    private bool isHoldingObject = false;
    private GameObject heldObject;
    private float pickupDistance = 3f;

    // Drop Crate
    private bool hasDroppedObject = false;

    // Capsule
    private float capsuleHeight = 2f;
    private float capsuleRadius = 2f;
    [SerializeField]
    private Transform holdPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isHoldingObject)
            {
                Debug.Log("Trying to pick up an object...");
                PickUpObject();
            }
            else
            {
                Debug.Log("Trying to drop the object...");
                DropObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isHoldingObject)
            {
                Debug.Log("Throwing the held object...");
                ThrowHeldObject();
            }
        }

        // Memastikan objek yang dipegang selalu berada di depan pemain
        if (isHoldingObject && heldObject != null)
        {
            Vector3 offset = transform.forward * 1.3f;

            heldObject.transform.position = transform.position + offset;
        }

        // Periksa apakah objek yang dipegang masih ada
        if (isHoldingObject && heldObject == null && !hasDroppedObject)
        {
            Debug.Log("The held object has been destroyed. Automatically dropping it.");
            DropObject();
            isHoldingObject = false;
            heldObject = null;
        }

    }

    // PickUp Object
    private void PickUpObject()
    {
        if (!isHoldingObject)
        {
            Collider[] colliders = Physics.OverlapCapsule(transform.position - Vector3.up * capsuleHeight / 2f, transform.position + Vector3.up * capsuleHeight / 2f, capsuleRadius);

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("crate"))
                {
                    // Perhitungan jarak antara pemain dan objek "crate"
                    float distance = Vector3.Distance(transform.position, col.transform.position);

                    if (distance <= pickupDistance)
                    {
                        CrateController crate = col.GetComponent<CrateController>();
                        if (crate != null && !crate.IsHeld)
                        {
                            crate.PickupObject(transform);
                            isHoldingObject = true;
                            heldObject = crate.gameObject;
                            break; // Hentikan iterasi setelah menemukan objek untuk diambil
                        }
                    }
                }
            }
        }
    }

    private void DropObject()
    {
        if (isHoldingObject && heldObject != null)
        {
            CrateController crate = heldObject.GetComponent<CrateController>();
            if (crate != null)
            {
                crate.DropObject();
                isHoldingObject = false;
                heldObject = null;
            }
        }
    }

    private void ThrowHeldObject()
    {
        if (isHoldingObject && heldObject != null)
        {
            CrateController crate = heldObject.GetComponent<CrateController>();
            if (crate != null)
            {
                // Kalkulasi arah dorongan berdasarkan arah pandangan pemain
                Vector3 throwDirection = transform.forward;

                // Panggil metode ThrowObject pada CrateController dengan arah dorongan yang tepat
                crate.ThrowObject(throwDirection);

                isHoldingObject = false;
                heldObject = null;
            }
        }
    }
}
