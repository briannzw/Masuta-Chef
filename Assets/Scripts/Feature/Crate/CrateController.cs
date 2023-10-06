using UnityEngine;

public class CrateController : MonoBehaviour, IPickable
{
    private bool isHeld = false;
    private Transform holder;
    private Rigidbody rb;
    private int originalLayer;

    public Transform Holder
    {
        get { return holder; }
        set { holder = value; }
    }

    public float ThrowForce
    {
        get { return throwForce; }
    }

    [SerializeField] private float throwForce = 15f;
    public bool IsHeld { get { return isHeld; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalLayer = gameObject.layer; // Menyimpan layer asli saat inisialisasi
    }

    public void StartPickup(GameObject picker)
    {
        // Memindahkan objek ke pemegang (picker)
        holder = picker.transform;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        isHeld = true;

        // Set lapisan menjadi "heldCrate" saat dipegang
        gameObject.layer = LayerMask.NameToLayer("heldCrate");
    }

    public void ExitPickup()
    {
        if (isHeld)
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.None;
        }

        holder = null;
        isHeld = false;

        // Kembalikan objek ke lapisan aslinya
        gameObject.layer = originalLayer;
    }

    public void Throw()
    {
        if (isHeld)
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.None;

            // Hitung sudut lemparan yang diinginkan dalam derajat
            float throwAngleInDegrees = 300f; // Ubah nilai ini sesuai dengan sudut yang diinginkan

            // Hitung vektor gaya yang diperlukan untuk melempar ke atas dengan sudut
            Vector3 throwDirection = Quaternion.Euler(throwAngleInDegrees, 0f, 0f) * Vector3.forward;

            // Terapkan gaya untuk melempar objek
            rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

            holder = null;
            isHeld = false;

            // Kembalikan objek ke lapisan aslinya
            gameObject.layer = originalLayer;
        }
    }

}
