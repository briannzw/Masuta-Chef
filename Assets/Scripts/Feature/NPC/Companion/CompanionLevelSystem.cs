using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionLevelSystem : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int maxLevel;
    [SerializeField] private int expPerEnemyKill;

    private int currentLevel;
    private int currentExp;

    void Start()
    {
        currentLevel = 1; // Start at level 1
        currentExp = 0;  // Initialize current experience points
        GameManager.Instance.OnEnemiesKilled += AddExperience;

        // Add experience relative to the survived time
        InvokeRepeating("ExpGainedPerSecond", 1f, 1f);
    }
    void Update()
    {
        LevelProgression();
    }

    private void LevelProgression()
    {
        // Iterated Version
        // int targetExp = currentLevel * 10;
        
        int targetExp = 10;

        // Check if the current experience points exceed the target for the next level
        if (currentExp >= targetExp && currentLevel < maxLevel)
        {
            int excessExp = currentExp - targetExp; // Calculate excess experience points
            currentLevel++;           // Increase the level
            currentExp = excessExp;   // Carry over excess experience points to the next level
        }
        else if (currentLevel >= maxLevel)
        {
            // Handle the case where the companion has reached the maximum level
            currentExp = 0;
        }
    }

    // Method to add experience points to the companion per second passed
    private void ExpGainedPerSecond()
    {
        currentExp += 1;
    }

    // Method to add experience points to the companion per enemy killed
    public void AddExperience()
    {
        currentExp += expPerEnemyKill;
    }
}
