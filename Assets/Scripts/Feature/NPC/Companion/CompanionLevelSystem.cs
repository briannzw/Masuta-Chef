using UnityEngine;
using Character.Stat;

public class CompanionLevelSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Character.Character character;

    [Header("Properties")]
    [SerializeField] private int maxLevel;
    [SerializeField] private int expPerEnemyKill;

    [Header("Level Stats")]
    public StatsPreset Stage1;
    public StatsPreset Stage2;
    public StatsPreset Stage3;

    private int currentLevel;
    private int currentExp;

    private void Awake()
    {
        if (character == null) character = GetComponent<Character.Character>();
    }

    void Start()
    {
        currentLevel = 1; // Start at level 1
        currentExp = 0;  // Initialize current experience points
        GameManager.Instance.OnEnemiesKilled += () => AddExperience(expPerEnemyKill);

        // Add experience relative to the survived time
        InvokeRepeating("ExpGainedPerSecond", 1f, 1f);
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

            // Character from Stage (each 10 level)
            if (currentLevel % 10 == 0)
            {
                character.StatsPreset = Mathf.RoundToInt(currentLevel / 10) == 1 ? Stage1 : Mathf.RoundToInt(currentLevel / 10) == 2 ? Stage2 : Stage3;
                character.Reset();

                // TODO: Add VFXs after leveling up
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
        AddExperience(1);
    }

    // Method to add experience points to the companion per enemy killed
    public void AddExperience(int exp)
    {
        currentExp += exp;

        // Call only on every exp added
        LevelProgression();
    }
}
