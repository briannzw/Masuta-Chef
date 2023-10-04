using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Transform GridCellPrefab;

    [SerializeField] private int height;
    [SerializeField] private int width;

    private Node[,] nodes;
    private bool[,] isCellOccupied; // Menyimpan informasi apakah sel sudah terisi atau tidak
    private int filledCellCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateGrid()
    {
        float cellSize = 1.0f; // Ukuran sel
        float spacing = 0.2f; // Spasi antara sel-sel

        nodes = new Node[width, height]; // Mengatur ukuran nodes menjadi (3,3)
        isCellOccupied = new bool[width, height]; // Inisialisasi array isCellOccupied

        var name = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 worldPosition = new Vector3(i * (cellSize + spacing), 0.51f, j * (cellSize + spacing));
                Transform obj = Instantiate(GridCellPrefab, worldPosition, Quaternion.identity, transform);
                obj.name = "Cell" + name;

                // Hanya mengatur nodes untuk sel yang berada dalam grid yang diinginkan (2x2)
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

    public int GetFilledCellCount()
    {
        return filledCellCount;
    }

}


