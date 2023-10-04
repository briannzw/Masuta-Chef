using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controller
{
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
        private GameObject heldObject; // Objek yang sedang dipegang
        private GameObject nearestPickable;

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
            if (!isHoldingObject && heldObject == null)
            {
                // Jika tidak memegang objek, coba mengambil objek terdekat
                if (nearestPickable != null)
                {
                    // Memeriksa apakah objek terdekat implementasi IPickable
                    IPickable pickable = nearestPickable.GetComponent<IPickable>();

                    if (pickable != null)
                    {        
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
                                float distanceToGrid = Vector3.Distance(heldObject.transform.position, nearestCell.cellPosition);

                                if (distanceToGrid < 3f)
                                {
                                    // Menandai sel yang akan diisi sebagai terisi
                                    nearestCell.isPlaceable = false;
                                    crateArea.AddFilledCells();
                                    // Jika jarak kurang dari snapToGridDistance, snap objek ke sel grid
                                    heldObject.transform.position = new Vector3(nearestCell.cellPosition.x, 1f, nearestCell.cellPosition.z);
                                    heldObject.transform.rotation = Quaternion.identity;
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
                IPickable pickable = heldObject.GetComponent<IPickable>();
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

                    // Mengatur kembali nilai-nilai isHoldingObject dan heldObject
                    isHoldingObject = false;
                    heldObject = null;

                    return;
                }
            }
        }


        #endregion

        private void OnDrawGizmos()
        {
            // Gambar gizmo untuk menunjukkan jangkauan pickup
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, PickupRadius);
        }
    }
}
