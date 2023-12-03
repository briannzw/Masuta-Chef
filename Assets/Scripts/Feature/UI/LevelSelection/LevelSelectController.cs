using UnityEngine;
using UnityEngine.UI;

namespace LevelSelection
{
    using Level;
    public class LevelSelectController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelData levelData;

        [Header("UI References")]
        [SerializeField] private Button levelButton;

        [Header("Lock")]
        [SerializeField] private bool isLocked = false;
        [SerializeField] private GameObject lockObj;

        private void Awake()
        {
            if (levelData == null)
            {
                Lock(true);
                return;
            }

            Lock(isLocked);
        }

        private void Lock(bool value)
        {
            lockObj.SetActive(value);
            levelButton.interactable = !value;
        }
    }
}