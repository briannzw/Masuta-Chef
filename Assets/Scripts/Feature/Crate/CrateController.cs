using UnityEngine;

public class CrateController : MonoBehaviour
{
    private bool isHeld = false;
    private Transform holder;
    private Rigidbody rb;
    private int originalLayer; // Menyimpan layer asli objek

    public float throwForce = 10f; // Kekuatan dorongan ketika crate dilempar

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalLayer = gameObject.layer; // Menyimpan layer asli saat inisialisasi
    }

    public bool IsHeld { get { return isHeld; } }

    public void PickupObject(Transform newHolder)
    {
        holder = newHolder;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        isHeld = true;

        // Set lapisan menjadi "heldCrate" saat dipegang
        gameObject.layer = LayerMask.NameToLayer("heldCrate");
        gameObject.tag = "crate";
    }

    public void DropObject()
    {
        if (isHeld)
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.None;

            Debug.Log("Crate Dropped");
        }

        holder = null;
        isHeld = false;

        // Kembalikan objek ke lapisan aslinya
        gameObject.layer = originalLayer;
    }

    public void ThrowObject(Vector3 throwDirection)
    {
        if (isHeld)
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.None;

            // Berikan gaya dorongan sesuai dengan arah pandangan pemain
            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            Debug.Log("Crate Thrown");
        }

        holder = null;
        isHeld = false;

        // Kembalikan objek ke lapisan aslinya
        gameObject.layer = originalLayer;
    }

}
