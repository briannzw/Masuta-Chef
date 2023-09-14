using System.Collections;
using TMPro;
using UnityEngine;

namespace Cooking.Gameplay.TapNumber
{
    public class CookingNumberController : MonoBehaviour
    {
        [Header("References")]
        public CookingNumberManager Manager;
        public TMP_Text NumberText;
        public float Duration;

        private void Start()
        {
            StartCoroutine(Countdown());
        }

        public void OnClick()
        {
            Manager.TapSuccess(int.Parse(NumberText.text));
            // Some VFX(s)
            Destroy(gameObject);
        }   

        private IEnumerator Countdown()
        {
            yield return new WaitForSeconds(Duration);
            Manager.TapMissed();
            Destroy(gameObject);
        }
    }
}