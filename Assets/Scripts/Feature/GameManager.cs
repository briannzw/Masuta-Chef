using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    public static Transform playerTransform;

    private void Awake()
    {
        // Set the static playerTransform variable to this player's transform
        playerTransform = player;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
