using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    private bool isHeld = false;

    private Transform holder;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public bool IsHeld { get { return isHeld; } }

    public void PickupObject(Transform newHolder)
    {
        holder = newHolder;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        isHeld = true;
    }

    public void DropObject()
    {
        if (isHeld)
        {
            // Kubus jatuh seperti biasa jika tidak memenuhi kondisi di atas
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.None;
            Debug.Log("Crate Drop");
        }

        holder = null;
        isHeld = false;
    }
}
