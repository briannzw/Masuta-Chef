using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateArea : MonoBehaviour
{

    public Transform GridCellPrefab;

    [SerializeField] private int height;
    [SerializeField] private int width;

    private Node[,] nodes;
    private bool[,] isCellOccupied; // Menyimpan informasi apakah sel sudah terisi atau tidak
    private int filledCellCount = 0;

    private ItemType currentState;

    public enum ItemType
    {
        None,
        Item,
        Medkit,
        Weapon,
        Companion
    }

    private bool isPlayerNearGrid = false; // Flag untuk menandai apakah pemain berada di dekat grid
    private bool wasPlayerNearGrid = false; // Flag untuk menyimpan status sebelumnya

    private void Start()
    {
        CreateGrid();
    }

    private void Update()
    {
        UpdateCurrentState(filledCellCount);

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
                Debug.Log("Player is no longer near the grid.");
            }

            wasPlayerNearGrid = isPlayerNearGrid; // Simpan status sebelumnya
        }
    }

    private void UpdateCurrentState(int filledCellCount)
    {
        // Set currentState sesuai dengan filledCellCount
        if (filledCellCount == 0)
        {
            currentState = ItemType.None;
        }
        else if (filledCellCount == 1)
        {
            currentState = ItemType.Item;
        }
        else if (filledCellCount == 2)
        {
            currentState = ItemType.Medkit;
        }
        else if (filledCellCount == 3)
        {
            currentState = ItemType.Weapon;
        }
        else if (filledCellCount >= 4)
        {
            currentState = ItemType.Companion;
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

    private void CreateGrid()
    {
        float cellSize = 1.0f; // Ukuran sel
        float spacing = 0.2f; // Spasi antara sel-sel

        nodes = new Node[width, height]; 
        isCellOccupied = new bool[width, height]; // Inisialisasi array isCellOccupied

        var name = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 worldPosition = new Vector3(i * (cellSize + spacing), 0.51f, j * (cellSize + spacing));
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

    public void AddFilledCells()
    {
        filledCellCount = filledCellCount + 1;

        // Debug log untuk menampilkan jumlah sel yang sudah terisi
        Debug.Log("Filled Cells: " + filledCellCount + "/" + (width * height));
    }

    public void DropFilledCells()
    {
        filledCellCount = filledCellCount - 1;

        // Debug log untuk menampilkan jumlah sel yang sudah terisi
        Debug.Log("Filled Cells: " + filledCellCount + "/" + (width * height));
    }

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

    public ItemType GetCurrentState()
    {
        return currentState;
    }
    public bool IsPlayerNearGrid()
    {
        return isPlayerNearGrid;
    }
    #endregion
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