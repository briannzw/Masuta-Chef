using UnityEngine;
using System.Collections;

public class LootSpawnManager : MonoBehaviour
{
    public GameObject weaponLootPrefab;
    public Transform playerTransform; // Referensi ke pemain atau objek tempat pemain berada
    public float spawnDistance = 4.0f; // Jarak spawn dari pemain

    public void SpawnWeaponLootNearPlayer()
    {
        Vector3 randomOffset = Random.insideUnitSphere * spawnDistance;
        randomOffset.y = 0; // Pastikan objek tetap di lantai
        Vector3 spawnPosition = playerTransform.position + randomOffset;

        Instantiate(weaponLootPrefab, spawnPosition, Quaternion.identity);
    }
}