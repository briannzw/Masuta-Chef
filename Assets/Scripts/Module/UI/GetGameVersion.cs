using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetGameVersion : MonoBehaviour
{
    [SerializeField] private TMP_Text versionText;

    private void Start()
    {
        versionText.text += Application.version;
    }
}
