using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Player.Input;

namespace Area.Interactable
{
    using Interaction;
    public class CrateArea : PlayerInputControl, IInteractable
    {
        [Header("Grid")]
        public Transform GridCellPrefab;
        [SerializeField] private int height;
        [SerializeField] private int width;
        private Node[,] nodes;
        private bool[,] isCellOccupied;

        [Header("Crate Grid")]
        public List<GameObject> CrateGrid = new List<GameObject>();
        private bool isPlayerNearGrid = false; // Flag untuk menandai apakah pemain berada di dekat grid
        private bool wasPlayerNearGrid = false; // Flag untuk menyimpan status sebelumnya

        private IconManager iconManager;
        LootSpawnManager lootSpawnManager;

        protected override void Start()
        {
            iconManager = FindObjectOfType<IconManager>();
            lootSpawnManager = FindObjectOfType<LootSpawnManager>();
            base.Start();
            CreateGrid();
        }

        private void Update()
        {
            DetectPlayerNearGrid();

            // Periksa perubahan status dan keluarkan debug jika pemain memasuki atau meninggalkan area grid
            if (isPlayerNearGrid != wasPlayerNearGrid)
            {
                if (isPlayerNearGrid)
                {
                    Debug.Log("Player is near the grid!");
                }
                else
                {
                    iconManager.MatikanSemuaIkon();

                }

                wasPlayerNearGrid = isPlayerNearGrid; // Simpan status sebelumnya
            }
        }

        private void CreateGrid()
        {
            float cellSize = 1; // Ukuran sel
            float spacing = 0.2f; // Spasi antara sel-sel

            nodes = new Node[width, height];
            isCellOccupied = new bool[width, height]; // Inisialisasi array isCellOccupied

            var name = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3 worldPosition = new Vector3(i * (cellSize + spacing) + (-0.57f), 2.51f, j * (cellSize + spacing) + 19.4f);
                    Transform obj = Instantiate(GridCellPrefab, worldPosition, Quaternion.identity, transform);
                    obj.name = "Cell" + name;

                    if (i < 2 && j < 2)
                    {
                        nodes[i, j] = new Node(true, worldPosition, obj);
                    }

                    // Awalnya, semua sel dianggap tidak terisi
                    isCellOccupied[i, j] = false;
                    name++;
                }
            }
        }

        public void Interact(GameObject other = null)
        {
            Debug.Log("Interacted with " + name);

            // Cek apakah objek lain memiliki komponen CrateController
            CrateController crateController = other.GetComponent<CrateController>();

            if (crateController != null)
            {
                Debug.Log("CrateColor: " + crateController.crateColor);
            }

            CrateGrid.Add(other);
            Debug.Log("Crate added to CrateGrid.");
        }

        // Metode untuk menghapus objek dari CrateGrid
        public void RemoveFromCrateGrid(GameObject crate)
        {
            if (CrateGrid.Contains(crate))
            {
                CrateGrid.Remove(crate);
                Debug.Log("Crate removed from CrateGrid.");
            }
        }

        // Metode ini akan dipanggil ketika objek lain memasuki collider area ini
        public void OnTriggerEnter(Collider other)
        {
            // Jika objek yang memasuki collider adalah crate, lakukan interaksi
            if (other.gameObject.layer == LayerMask.NameToLayer("Crate"))
            {
                Interact(other.gameObject);
            }
        }

        private void DetectPlayerNearGrid()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 4f);

            bool playerFound = false; // Flag untuk menandai apakah pemain ditemukan

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // Pemain berada di dekat grid
                    playerFound = true;
                    break;
                }
            }

            // Perbarui status pemain dekat grid
            isPlayerNearGrid = playerFound;
        }

        #region Callback
        protected override void RegisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Combine.performed += OnCombine;
        }

        protected override void UnregisterInputCallbacks()
        {
            if (playerControls == null) return;

            playerControls.Gameplay.Combine.performed -= OnCombine;
        }
        #endregion

        #region Callback Function
        private void OnCombine(InputAction.CallbackContext context)
        {
            iconManager.MatikanSemuaIkon();

            int crateCount = CrateGrid.Count;
            int greenCount = 0;
            int blueCount = 0;
            int redCount = 0;

            // Loop melalui semua Crate dalam CrateGrid dan hitung jumlahnya berdasarkan warna
            foreach (var crate in CrateGrid)
            {
                CrateController crateController = crate.GetComponent<CrateController>();
                if (crateController != null)
                {
                    if (crateController.crateColor == CrateController.CrateColor.Green)
                    {
                        greenCount++;
                    }
                    else if (crateController.crateColor == CrateController.CrateColor.Blue)
                    {
                        blueCount++;
                    }
                    else if (crateController.crateColor == CrateController.CrateColor.Red)
                    {
                        redCount++;
                    }
                }
            }

            if (crateCount == 1)
            {
                iconManager.iconItem.SetActive(true);
            }
            if (crateCount == 2)
            {
                iconManager.iconMedkit.SetActive(true);
            }
            if (crateCount == 3)
            {
                if (redCount == crateCount)
                {
                    iconManager.iconWeapon.SetActive(true);
                }
                else if (greenCount > 0 || blueCount > 0)
                {
                    // Jika ada kotak biru atau hijau, munculkan secara acak antara iconWeapon2 dan iconWeapon3
                    int randomValue = Random.Range(0, 2); // Menghasilkan 0 atau 1 secara acak
                    if (randomValue == 0)
                    {
                        iconManager.iconWeapon3.SetActive(true);
                    }
                    else
                    {
                        iconManager.iconWeapon2.SetActive(true);
                    }
                }
                // Spawn weaponLootPrefab dekat pemain
                lootSpawnManager.SpawnWeaponLootNearPlayer();
            }
            if (crateCount == 4)
            {
                if (redCount == crateCount)
                {
                    iconManager.iconCompanion.SetActive(true);
                }
                else if (greenCount > 0 || blueCount > 0)
                {
                    // Jika ada kotak biru atau hijau, munculkan secara acak antara iconWeapon2 dan iconWeapon3
                    int randomValue = Random.Range(0, 2); // Menghasilkan 0 atau 1 secara acak
                    if (randomValue == 0)
                    {
                        iconManager.iconCompanion3.SetActive(true);
                    }
                    else
                    {
                        iconManager.iconCompanion2.SetActive(true);
                    }
                }
            }
            // Hapus semua objek dalam CrateGrid sebelum membersihkan list
            foreach (var crate in CrateGrid)
            {
                Destroy(crate);
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Node cell = GetNodeAt(i, j);
                    if (cell != null)
                    {
                        cell.isPlaceable = true;
                    }
                }
            }
            // Hapus semua elemen dari CrateGrid setelah menjalankan logika "combine"
            CrateGrid.Clear();
            // Implementasikan logika "combine" di sini.
            Debug.Log("Player has performed combine action.");
        }
        #endregion

        #region Getter
        public Node GetNodeAt(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return nodes[x, y];
            }
            return null;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }
        public bool IsPlayerNearGrid()
        {
            return isPlayerNearGrid;
        }
        #endregion
    }
}


public class Node
{
    public bool isPlaceable;
    public Vector3 cellPosition;
    public Transform obj;

    public Node(bool isPlaceable, Vector3 cellPosition, Transform obj)
    {
        this.isPlaceable = isPlaceable;
        this.cellPosition = cellPosition;
        this.obj = obj;
    }
}