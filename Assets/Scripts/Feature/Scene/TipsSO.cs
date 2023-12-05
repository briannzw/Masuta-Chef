using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tips/New Tips List", fileName = "New TipsSO")]
public class TipsSO : ScriptableObject
{
    [TextArea] public List<string> Tips = new();

    public string RandomTips()
    {
        return Tips[Random.Range(0, Tips.Count)];
    }
}
