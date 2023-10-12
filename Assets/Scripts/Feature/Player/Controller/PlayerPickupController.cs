using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Linq;

namespace Player.Controller
{
    using Area.Interactable;
    using Input;
    public class PlayerPickupController : PlayerInputControl
    {
        [Header("References")]
        public Animator Animator;

        [Header("Pickup Settings")]
        public float PickupRadius = 3f;
        [Range(0, 360)] public float PickupAngle = 125;
        public float ThrowForce = 10f;
        public LayerMask TargetMask;
        private bool isHoldingObject = false;
        private bool isPlayerNearGrid;
        private bool isCheckingObjectInGrid = false;
        private GameObject heldObject; // Objek yang sedang dipegang
        private GameObject nearestPickable;
        private IPickable pickable;

        [Header("Capsule")]
        public float capsuleHeight = 2f;
        public float capsuleRadius = 1f;
        public Transform holdPosition;

        private CrateArea crateArea;

        protected override void Start()
        {
            crateArea = FindObjectOfType<CrateArea>();

            base.Start();
        }


        private void Update()
        {
            // Implementasikan logika untuk menemukan objek terdekat yang dapat diambil
            Collider[] colliders = Physics.OverlapCapsule(transform.position - Vector3.up * capsuleHeight, transform.position + Vector3.up * capsuleHeight, PickupRadius, TargetMask);

            nearestPickable = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider col in colliders)
            {
                Vector3 toPickup = col.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, toPickup);

                if (angle <= PickupAngle)
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);

                    if (distance < nearestDistance)
                    {
                        nearestPickable = col.gameObject;
                        nearestDistance = distance;
                    }
                }
            }

            Node nearestCell = FindNearestCellToPlayer();
            if (!nearestCell.isPlaceable)
            {
                // Jika sel sudah terisi, cari sel kosong terdekat
                nearestCell = FindNearestEmptyCell(nearestCell);
            }
            if (nearestCell != null)
            {
                // Debug log hanya jika pemain menghadap ke sel terdekat
                Vector3 toCell = nearestCell.cellPosition - transform.position;
                float angleToCell = Vector3.Angle(transform.forward, toCell);
                // Debug log informasi sel terdekat
                //Debug.Log("Nearest Cell Position: " + nearestCell.obj.name);
            }

            // Memastikan objek yang dipegang selalu berada di depan pemain
            if (isHoldingObject && heldObject != null)
            {
                Vector3 offset = transform.forward * 1.4f;

                heldObject.transform.position = transform.position + offset;
            }

        }

        #region Find Nearest
        private Node FindNearestCellToPlayer()
        {
            if (crateArea != null)
            {
                Node nearestCell = null;
                float nearestDistance = float.MaxValue;
                int gridWidth = crateArea.GetWidth();
                int gridHeight = crateArea.GetHeight();

                for (int i = 0; i < gridWidth; i++)
                {
                    for (int j = 0; j < gridHeight; j++)
                    {
                        Node cell = crateArea.GetNodeAt(i, j);
                        if (cell != null)
                        {
                            float distance = Vector3.Distance(transform.position, cell.cellPosition);

                            if (distance < nearestDistance)
                            {
                                nearestCell = cell;
                                nearestDistance = distance;
                            }
                        }
                    }
                }
                return nearestCell;
            }
            return null;
        }

        // Method untuk mencari sel kosong terdekat
        private Node FindNearestEmptyCell(Node currentCell)
        {
            int maxDistance = 1; // Maksimum jarak pencarian sel kosong

            for (int distance = 1; distance <= maxDistance; distance++)
            {
                for (int i = -distance; i <= distance; i++)
                {
                    for (int j = -distance; j <= distance; j++)
                    {
                        int x = Mathf.RoundToInt(currentCell.cellPosition.x) + i;
                        int z = Mathf.RoundToInt(currentCell.cellPosition.z) + j;

                        Node cell = crateArea.GetNodeAt(x, z);
                        if (cell != null && cell.isPlaceable)
                        {
                            return cell; // Mengembalikan sel kosong terdekat yang ditemukan
                        }
                    }
                }
            }

            return null; // Tidak ada sel kosong yang ditemukan dalam jarak tertentu
        }

        #endregion

        #region Callback
        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.PickUp.performed += OnPickUp;
            playerControls.Gameplay.Throw.performed += OnThrow;
        }
        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.PickUp.performed -= OnPickUp;
            playerControls.Gameplay.Throw.performed -= OnThrow;
        }
        #endregion

        #region Callback Function
        private void OnPickUp(InputAction.CallbackContext context)
        {
            GameObject snapAreaObject = GameObject.Find("AreaCollider");

            if (!isHoldingObject && heldObject == null)
            {

                // Jika tidak memegang objek, coba mengambil objek terdekat
                if (nearestPickable != null)
                {
                    // Memeriksa apakah objek terdekat implementasi IPickable
                    IPickable pickable = nearestPickable.GetComponent<IPickable>();

                    if (pickable != null)
                    {
                        if (pickable.Holder != null && pickable.Holder.name == "AreaCollider")
                        {
                            // Lakukan sesuatu jika Holder adalah "AreaCollider"
                            Debug.Log("Holder adalah AreaCollider");
                            // Hapus objek dari CrateGrid di crateArea
                            if (crateArea != null)
                            {
                                // Hapus objek yang diambil dari CrateGrid
                                crateArea.RemoveFromCrateGrid(nearestPickable);
                                Node nearestCell = FindNearestCellToPlayer();
                                nearestCell.isPlaceable = true;

                                Debug.Log("Yey remooovee");
                            }
                            else { Debug.Log("null nih bosque"); }
                        }

                        // Memulai pengambilan objek
                        pickable.StartPickup(gameObject);

                        // Set heldObject dengan objek yang dipegang
                        heldObject = nearestPickable;
                        heldObject.transform.parent = holdPosition;
                        heldObject.transform.localPosition = Vector3.zero;

                        // Debug log untuk memberi tahu bahwa objek diambil
                        Debug.Log("Picked up: " + heldObject.name);

                        // Set isHoldingObject ke true
                        isHoldingObject = true;
                    }
                }
            }
            else if (isHoldingObject && heldObject != null)
            {
                // Jika sudah memegang objek, cek apakah pemain ingin melepas objek
                // Anda dapat menambahkan input tambahan di sini untuk mengatur kondisi pelepasan objek
                if (context.performed)
                {
                    // Panggil fungsi DetectPlayerNearGrid dari CombineCrate untuk memeriksa apakah pemain dekat dengan grid
                    if (crateArea != null)
                    {
                        isPlayerNearGrid = crateArea.IsPlayerNearGrid();
                    }

                    Node nearestCell = FindNearestCellToPlayer();
                    if (nearestCell != null && !nearestCell.isPlaceable)
                    {
                        // Jika sel sudah terisi, cari sel kosong terdekat
                        nearestCell = FindNearestEmptyCell(nearestCell);
                    }

                    if (nearestCell != null || nearestCell == null && !isPlayerNearGrid)
                    {
                        // Debug log untuk memberi tahu bahwa objek dilepas
                        Debug.Log("Dropped: " + heldObject.name);

                        // Release the object from the player's transform
                        heldObject.transform.parent = null;

                        // Memeriksa apakah objek yang dipegang implementasi IPickable
                        IPickable pickable = heldObject.GetComponent<IPickable>();

                        if (pickable != null)
                        {
                            // Melepaskan objek
                            pickable.ExitPickup();
                            if (nearestCell != null)
                            {
                                // Periksa jarak dari objek ke sel grid terdekat
                                if (isPlayerNearGrid)
                                {
                                    // Menandai sel yang akan diisi sebagai terisi
                                    nearestCell.isPlaceable = false;
                                    // Jika jarak kurang dari snapToGridDistance, snap objek ke sel grid
                                    heldObject.transform.position = new Vector3(nearestCell.cellPosition.x, 3f, nearestCell.cellPosition.z);
                                    heldObject.transform.rotation = Quaternion.identity;

                                    // Mengambil referensi ke objek "AreaCollider" dalam hierarki
                                    pickable.Holder = snapAreaObject.transform;
                                }
                            }
                            else
                            {
                                // Jika jarak lebih besar dari snapToGridDistance, biarkan objek jatuh seperti biasa
                                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                                if (rb != null)
                                {
                                    rb.isKinematic = false;
                                }
                            }
                            // Mengatur kembali nilai-nilai isHoldingObject dan heldObject
                            isHoldingObject = false;
                            heldObject = null;
                        }
                    }
                }

            }
        }


        private void OnThrow(InputAction.CallbackContext context)
        {
            if (isHoldingObject && heldObject != null)
            {
                // Release the object from the player's transform
                heldObject.transform.parent = null;

                // Melepaskan objek setelah melempar
                pickable = heldObject.GetComponent<IPickable>();
                if (pickable != null)
                {
                    // Debug log untuk memberi tahu bahwa objek dilempar
                    Debug.Log("Thrown: " + heldObject.name);

                    // Implementasi logika melempar objek di sini jika diperlukan
                    // Anda dapat mengambil komponen Rigidbody objek yang dipegang dan memberikannya kecepatan untuk melempar objek
                    Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }

                    pickable.Throw();
                    // Mulai coroutine untuk memeriksa apakah objek masuk ke dalam daftar CrateGrid selama 4 detik
                    StartCoroutine(CheckObjectInCrateGridForSeconds(3f));

                    isHoldingObject = false;
                 
                }
            }
        }
        #endregion

        private bool IsObjectInCrateGrid(GameObject obj)
        {
            return crateArea != null && crateArea.CrateGrid.Any(crate => crate == obj);
        }

        private IEnumerator CheckObjectInCrateGridForSeconds(float seconds)
        {
            isCheckingObjectInGrid = true;
            float startTime = Time.time;
            Node nearestCell = FindNearestCellToPlayer();
            if (nearestCell != null && !nearestCell.isPlaceable)
            {
                // Jika sel sudah terisi, cari sel kosong terdekat
                nearestCell = FindNearestEmptyCell(nearestCell);
            }
            GameObject snapAreaObject = GameObject.Find("AreaCollider");

            while (Time.time - startTime < seconds)
            {
                if (pickable != null)
                {
                    // Cek apakah objek yang dilempar ada dalam daftar CrateGrid
                    bool objectInCrateGrid = IsObjectInCrateGrid(heldObject);

                    Debug.Log("Checking if " + heldObject.name + " is in CrateGrid. Result: " + objectInCrateGrid);

                    if (objectInCrateGrid)
                    {
                        // Objek masuk ke dalam daftar CrateGrid, lakukan tindakan yang sesuai
                        Debug.Log("Object entered CrateGrid: " + heldObject.name);
                        // Menandai sel yang akan diisi sebagai terisi
                        nearestCell.isPlaceable = false;
                        // Jika jarak kurang dari snapToGridDistance, snap objek ke sel grid
                        heldObject.transform.position = new Vector3(nearestCell.cellPosition.x, 3f, nearestCell.cellPosition.z);
                        heldObject.transform.rotation = Quaternion.identity;

                        // Mengambil referensi ke objek "AreaCollider" dalam hierarki
                        pickable.Holder = snapAreaObject.transform;
                        isCheckingObjectInGrid = false; // Hentikan pemeriksaan setelah objek masuk
                        heldObject = null;
                        yield break;
                    }
                }
                else
                {
                    Debug.Log("heldobject ny nulll bye");
                }

                yield return null; // Tunggu frame selanjutnya
            }
            heldObject = null;
            isCheckingObjectInGrid = false;
        }

        private void OnDrawGizmos()
        {
            // Gambar gizmo untuk menunjukkan jangkauan pickup
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, PickupRadius);
        }
    }
}
