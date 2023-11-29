using UnityEngine;
using Character.Stat;
using System;

public class CompanionLevelSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Character.Character character;

    [Header("Properties")]
    [SerializeField] private int maxLevel;
    [SerializeField] private int expPerEnemyKill;

    [Header("Level Stats")]
    public Stats StatsPerLevel;

    private int currentLevel;
    private int currentExp;

    public Action OnLevelUp;
    public int CurrentLevel => currentLevel;

    private void Awake()
    {
        if (character == null) character = GetComponent<Character.Character>();
    }

    void OnEnable()
    {
        currentLevel = 1; // Start at level 1
        currentExp = 0;  // Initialize current experience points
        GameManager.Instance.OnEnemiesKilled += () => AddExperience(expPerEnemyKill);

        // Add experience relative to the survived time
        InvokeRepeating("ExpGainedPerSecond", 1f, 1f);
    }

    private void OnDisable()
    {
        CancelInvoke();
        GameManager.Instance.OnEnemiesKilled -= () => AddExperience(expPerEnemyKill);
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

            // Add Stats per level
            foreach(var stat in StatsPerLevel.StatList)
            {
                character.Stats.StatList[stat.Key].AddModifier(new Kryz.CharacterStats.StatModifier(stat.Value.Value, Kryz.CharacterStats.StatModType.Flat));
                if (stat.Key == StatsEnum.Speed) character.OnSpeedChanged?.Invoke();
            }

            foreach(var dynamicStat in StatsPerLevel.DynamicStatList)
            {
                character.Stats.DynamicStatList[dynamicStat.Key].AddModifier(new Kryz.CharacterStats.StatModifier(dynamicStat.Value.Value, Kryz.CharacterStats.StatModType.Flat));
            }

            OnLevelUp?.Invoke();

            // Stages (each 10 level)
            if (currentLevel % 10 == 0)
            {
                // TODO: Add VFXs after stage up
            }
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
        AddExperience(10);
    }

    // Method to add experience points to the companion per enemy killed
    public void AddExperience(int exp)
    {
        currentExp += exp;

        // Call only on every exp added
        LevelProgression();
    }
}
