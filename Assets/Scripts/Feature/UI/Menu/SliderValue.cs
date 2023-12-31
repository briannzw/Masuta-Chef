using TMPro;
using UnityEngine;

public class SliderValue : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    [SerializeField] private string suffix;

    public void UpdateText(float value)
    {
        m_Text.text = Mathf.RoundToInt(value * 100f).ToString() + suffix;
    }
}
