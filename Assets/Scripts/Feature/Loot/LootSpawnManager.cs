using UnityEngine;
using System.Collections;

public class LootSpawnManager : MonoBehaviour
{
    public GameObject IngredientLootPrefab;
    public GameObject MedkitLootPrefab;
    public GameObject weapon1LootPrefab;
    public GameObject weapon2LootPrefab;
    public GameObject weapon3LootPrefab;
    public GameObject Companion1LootPrefab;
    public GameObject Companion2LootPrefab;
    public GameObject Companion3LootPrefab;

    public Transform playerTransform;
    public float spawnDistance = 3.0f;
    public string LootObject;
    public float minimumDistanceToPlayer = 1.0f;

    public void SpawnLootNearPlayer()
    {
        Vector3 randomOffset;
        Vector3 spawnPosition;
        Vector3 playerPosition = playerTransform.position;

        do
        {
            float xOffset = Random.Range(-spawnDistance, spawnDistance);
            float zOffset = 0; 
            randomOffset = new Vector3(xOffset, 0, zOffset);

            spawnPosition = playerPosition + new Vector3(xOffset, 0, 0);
        } while (Vector3.Distance(spawnPosition, playerPosition) < minimumDistanceToPlayer);

        if (LootObject == "Ingredient")
        {
            Instantiate(IngredientLootPrefab, spawnPosition, Quaternion.identity);
        }
        if (LootObject == "Medkit")
        {
            Instantiate(MedkitLootPrefab, spawnPosition, Quaternion.identity);
        }
        if (LootObject == "Weapon1")
        {
            Instantiate(weapon1LootPrefab, spawnPosition, Quaternion.identity);
        }
        if (LootObject == "Weapon2")
        {
            Instantiate(weapon2LootPrefab, spawnPosition, Quaternion.identity);
        }
        if (LootObject == "Weapon3")
        {
            Instantiate(weapon3LootPrefab, spawnPosition, Quaternion.identity);
        }
        if (LootObject == "Companion1")
        {
            Instantiate(Companion1LootPrefab, spawnPosition, Quaternion.identity);
        }
        if (LootObject == "Companion2")
        {
            Instantiate(Companion2LootPrefab, spawnPosition, Quaternion.identity);
        }
        if (LootObject == "Companion3")
        {
            Instantiate(Companion3LootPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
