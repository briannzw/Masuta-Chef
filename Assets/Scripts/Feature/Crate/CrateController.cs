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
        public GameObject CurrentPicker;
        public bool RandomColorOnStart = false;
        public CrateColor crateColor;
        public Rigidbody rb { get; set; }
        private new Renderer renderer;
        // Tambahkan properti untuk kecepatan jatuh
        public float fallSpeed = 9.8f;
        // Tambahkan properti untuk ketinggian awal jatuh
        public float initialFallHeight = 20.0f;
        //Tambahkan audioclip 
        public AudioClip pickupClip;
        public AudioClip droppedClip;
        //tambahkan audiosource
        public AudioSource audioSource;
        [SerializeField] private ParticleSystem particleDropCrate;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            renderer = GetComponentInChildren<Renderer>();

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
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
            Vector3 initialPosition = transform.position;
            initialPosition.y = initialFallHeight;
            transform.position = initialPosition;
            // Berikan kecepatan awal ke bawah
            rb.velocity = new Vector3(0, -fallSpeed, 0);
        }
        public bool StartPickup(GameObject picker)
        {
            CurrentPicker = picker;
            if (IsHeld && !picker.CompareTag("Player") && !picker.CompareTag("Enemy")) return false;
            if (Holder != picker && Holder != null) Holder.GetComponent<IPicker>().OnStealed(this);
            // Memindahkan objek ke pemegang (picker)
            Holder = picker.transform;
            rb.isKinematic = true;

            // Play SFX (Pickup)
            PlaySFX(pickupClip);
            particleDropCrate.Clear();

            return true;
        }

        public void ExitPickup()
        {
            Holder = null;
            rb.isKinematic = false;

            
        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip != null && audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                particleDropCrate.Play();
                // Play SFX (Dropped)
                PlaySFX(droppedClip);
            }
        }
    }
}