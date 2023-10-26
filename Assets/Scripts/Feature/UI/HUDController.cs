using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character.Stat;

public class HUDController : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private ManaBar manaBar;

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
