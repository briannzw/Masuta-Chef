using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Crate
{
    using Pickup;

    public enum CrateColor { Red, Blue, Green };

    public class CrateController : MonoBehaviour, IThrowable
    {
        public Transform Holder { get; set; }
        public bool IsHeld => Holder != null;

        public bool RandomColorOnStart = false;
        public CrateColor crateColor;

        public Rigidbody rb { get; set; }
        private new Renderer renderer;

        // Tambahkan atribut untuk kecepatan jatuh lambat
        public float slowFallSpeed = 1.0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            renderer = GetComponent <Renderer>();
        }

        private void Start()
        {
            if (RandomColorOnStart) crateColor = (CrateColor)Random.Range(0, Enum.GetNames(typeof(CrateColor)).Length);
            // Mengatur warna krate sesuai dengan enum CrateColor
            switch (crateColor)
            {
                case CrateColor.Red:
                    renderer.material.color = Color.red;
                    break;
                case CrateColor.Blue:
                    renderer.material.color = Color.blue;
                    break;
                case CrateColor.Green:
                    renderer.material.color = Color.green;
                    break;
                default:
                    renderer.material.color = Color.white; // Warna default jika enum tidak sesuai
                    break;
            }
        }

        public bool StartPickup(GameObject picker)
        {
            if (IsHeld && !picker.CompareTag("Player")) return false;

            if (Holder != picker && Holder != null) Holder.GetComponent<IPicker>().OnStealed(this);

            // Memindahkan objek ke pemegang (picker)
            Holder = picker.transform;
            rb.isKinematic = true;

            return true;
        }

        public void ExitPickup()
        {
            Holder = null;
            rb.isKinematic = false;

            // Setelah objek krate dilepaskan, atur kecepatan jatuh lambat
            rb.velocity = new Vector3(rb.velocity.x, -slowFallSpeed, rb.velocity.z);
        }
    }
}
