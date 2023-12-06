using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking.Gameplay.TapNumber
{
    public class CookingNumberController : MonoBehaviour
    {
        [Header("References")]
        public CookingNumberManager Manager;
        public Image IngredientIcon;
        public TMP_Text NumberText;
        public Image TimingCircle;
        public Image AccuracyCircle;
        [SerializeField] private ParticleSystem hitEffect;

        [Header("Parameters")]
        public float Duration;
        public float TimingCircleScale;
        public float AccuracyCircleScale;

        private void Awake()
        {
            TimingCircle.rectTransform.localScale = Vector3.one * TimingCircleScale;

            IngredientIcon.enabled = false;

            if (CookingManager.Instance == null) return;

            // Random Icon based on CurrentRecipe on Awake
            var ingredients = new List<Ingredient>(CookingManager.Instance.CurrentRecipe.Ingredients.Keys);
            IngredientIcon.sprite = ingredients[Random.Range(0, ingredients.Count)].CookingIcon;
            IngredientIcon.enabled = true;
        }

        private void Start()
        {
            StartCoroutine(Countdown());
            StartCoroutine(DoScaleTiming());
        }

        private void OnValidate()
        {
            AccuracyCircle.rectTransform.localScale = Vector3.one * AccuracyCircleScale;
        }

        public void OnClick()
        {
            // Make sure that TimingCircle and AccuracyCircle Width and Height is the same.
            if (TimingCircle.rectTransform.localScale.x <= AccuracyCircleScale) Manager.TapSuccess(int.Parse(NumberText.text));
            else Manager.TapMissed(int.Parse(NumberText.text));
            Vector3 particlePos = transform.position + new Vector3(0, 0, -5);
            // Some VFX(s)
            Instantiate(hitEffect, particlePos, Quaternion.Euler(180f,0,0));
            Destroy(gameObject);
        }   

        private IEnumerator Countdown()
        {
            yield return new WaitForSeconds(Duration);
            Manager.TapMissed(int.Parse(NumberText.text));
            Destroy(gameObject);
        }

        private IEnumerator DoScaleTiming()
        {
            float time = 0;
            float endValue = 1f;
            Vector3 startScale = TimingCircle.rectTransform.localScale;
            while (time < Duration)
            {
                if(Manager.GameEnded) Destroy(gameObject);
                TimingCircle.rectTransform.localScale = Vector3.one * Mathf.Lerp(startScale.x, endValue, time / Duration);
                time += Time.deltaTime;
                yield return null;
            }

            TimingCircle.rectTransform.localScale = Vector3.one * endValue;
        }
    }
}