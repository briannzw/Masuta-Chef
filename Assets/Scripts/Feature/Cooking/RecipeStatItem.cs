using TMPro;
using UnityEngine;

namespace Cooking.Recipe.UI
{
    public class RecipeStatItem : MonoBehaviour
    {
        [Header("References")]
        public TMP_Text StatsText;

        [Header("Lock")]
        public GameObject LockedOverlay;
        public TMP_Text LockText;

        public void Lock()
        {
            LockedOverlay.SetActive(true);
        }

        public void Unlock()
        {
            LockedOverlay.SetActive(false);
        }
    }
}