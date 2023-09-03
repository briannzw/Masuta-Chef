using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public float fallSpeed = 2.0f; // Kecepatan jatuhnya objek
    public int maxSpawnCount = 5; // Jumlah maksimum objek yang dapat di-spawn
    private int currentSpawnCount = 0; // Jumlah objek yang sudah di-spawn

    void Update()
    {
        if (currentSpawnCount < maxSpawnCount && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, 11), 40, Random.Range(-10, 11)); // Munculkan dari atas
            GameObject newCube = Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);

            // Tambahkan Rigidbody jika belum ada
            Rigidbody cubeRigidbody = newCube.GetComponent<Rigidbody>();
            if (cubeRigidbody == null)
            {
                cubeRigidbody = newCube.AddComponent<Rigidbody>();
            }

            // Beri gaya gravitasi agar objek jatuh ke bawah
            cubeRigidbody.useGravity = true;
            cubeRigidbody.velocity = new Vector3(0, -fallSpeed, 0);

            currentSpawnCount++;
        }
    }
}
