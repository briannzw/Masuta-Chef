using Kryz.CharacterStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public KeyCode Key;
    public KeyCode Key2;
    public KeyCode Key3;
    public Character.Character OtherStats;
    public float DamageScaling = 1f;

    private Character.Character TempStats;

    private void Start()
    {
        TempStats = GetComponent<Character.Character>();
    }

    void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            OtherStats.TakeDamage(TempStats, DamageScaling);
        }
        if (Input.GetKeyDown(Key2))
        {
            OtherStats.Health.AddModifier(new StatModifier(0.1f, StatModType.PercentMult));
        }
        if(Input.GetKeyDown(Key3))
        {

        }
    }
}
