using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RandomExpandingBoxSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float initialBoxSize = 1f;
    public float maxBoxSize = 5f;
    public float expandSpeed = 1f;
    public float spawnDelay = 1f;
    public KeyCode spawnKeyCode = KeyCode.Space;

    private Vector3 targetBoxSize;
    private bool isExpanding = false;

    private void Start()
    {
        targetBoxSize = Vector3.one * initialBoxSize;
    }

    private void Update()
    {
        HandleInput();
        UpdateBoxSize();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(spawnKeyCode))
        {
            isExpanding = true;
        }
    }

    private void UpdateBoxSize()
    {
        if (isExpanding)
        {
            targetBoxSize += Vector3.one * expandSpeed * Time.deltaTime;
            targetBoxSize = Vector3.Min(targetBoxSize, Vector3.one * maxBoxSize);

            transform.localScale = targetBoxSize;

            if (targetBoxSize.magnitude >= maxBoxSize)
            {
                isExpanding = false;
                SpawnObject();
                targetBoxSize = Vector3.one * initialBoxSize;
            }
        }
    }

    private void SpawnObject()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        Rigidbody objectRigidbody = newObject.GetComponent<Rigidbody>();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 minBounds = transform.position - targetBoxSize / 2;
        Vector3 maxBounds = transform.position + targetBoxSize / 2;

        Vector3 randomPosition = new Vector3(
            Random.Range(minBounds.x, maxBounds.x),
            Random.Range(minBounds.y, maxBounds.y),
            Random.Range(minBounds.z, maxBounds.z)
        );

        return randomPosition;
    }
}




