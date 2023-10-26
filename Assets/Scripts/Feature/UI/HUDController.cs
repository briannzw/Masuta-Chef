using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Stat;
using System;

public class HUDController : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private ManaBar manaBar;
    public Stats playerStats;
    [SerializeField] private string playerTagToFind = "Player";

    private void Start()
    {
        GameObject player = GameObject.FindWithTag(playerTagToFind);
        if (player == null) 
        {
            Debug.LogWarning("Player are not found. HUD display will be inaccurate!");
            return;
        }

        playerStats = player.GetComponent<Character.Character>().Stats;
        if (playerStats == null)
        {
            Debug.LogWarning("Player Stats not found. HUD display will be inaccurate!");
            return;
        }

        Initialize(
            playerStats.DynamicStatList[DynamicStatsEnum.Health].Value,
            playerStats.DynamicStatList[DynamicStatsEnum.Mana].Value
        );
    }

    // This is a workaround. not an optimal solution
    private void FixedUpdate()
    {
        SetHealth(playerStats.DynamicStatList[DynamicStatsEnum.Health].Value);
        SetMana(playerStats.DynamicStatList[DynamicStatsEnum.Mana].Value);
    }

    public void Initialize(
        float maxHealth,
        float maxMana
    ) {
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);

        manaBar.SetMaxMana(maxMana);
        manaBar.SetMana(maxMana);
    }

    public void SetHealth(float health)
    {
        if (health > healthBar.GetMaxHealth())
        {
            healthBar.SetHealth(healthBar.GetMaxHealth());
        }
        else if (health < 0)
        {
            healthBar.SetHealth(0);
        }
        else
        {
            healthBar.SetHealth(health);
        }
    }

    public void SetMana(float mana)
    {
        if (mana > manaBar.GetMaxMana()) manaBar.SetMana(manaBar.GetMaxMana());
        else if (mana < 0) manaBar.SetMana(0);
        else manaBar.SetMana(mana);
    }
    private void Update() {
        if (Input.GetKey(KeyCode.L)) Initialize(100, 100);

        if (Input.GetKey(KeyCode.K)) SetHealth(80);

        if (Input.GetKey(KeyCode.J)) SetHealth(70);
    }
}
