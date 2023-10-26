using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;

    public float GetMaxMana()
    {
        return slider.maxValue;
    }
    
    public void SetMana(float mana)
    {
        slider.value = mana;
    }

    public void SetMaxMana(float mana)
    {
        slider.maxValue = mana;
    }
}
