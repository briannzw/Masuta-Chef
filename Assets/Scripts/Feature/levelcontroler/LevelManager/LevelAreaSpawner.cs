using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace levelareaspawner
{ 
public class LevelAreaSpawner : MonoBehaviour
  {
    public GameObject levelAreaPrefab;

    void Start()
    {
        // Hasilkan prefab di posisi GameObject kosong ini
        Instantiate(levelAreaPrefab, transform.position, transform.rotation);
    }
  }
}
