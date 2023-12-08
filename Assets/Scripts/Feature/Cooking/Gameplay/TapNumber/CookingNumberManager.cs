using System;
using UnityEngine;

namespace Cooking.Gameplay.TapNumber
{
    using AYellowpaper.SerializedCollections;
    using Cooking.Gameplay.UI;
    using TMPro;
    using UnityEngine.UI;

    public class CookingNumberManager : CookingGameplay
    {
        [Header("Indicator UI")]
        public CookingIndicator CookingIndicator;
        public SerializedDictionary<CookingResult, float> AccuracyPercentages = new();

        [Header("UI References")]
        [SerializeField] private Image gameTimeImage;
        [SerializeField] private TMP_Text accuracyText;

        [Header("Parameters")]
        public float GameTime = 60f;

        private float timer = 0f;
        public bool GameEnded = false;

        private int successTap = 0;
        private int missedTap = 0;

        private int lastTapNum = -1;

        private void Awake()
        {
            CookingIndicator.AccuracyPercentages = AccuracyPercentages;
            // Set to PERFECT
            CookingIndicator.SetIndicatorUI(1f);
        }

        private void Update()
        {
            if(GameEnded) return;

            timer += Time.deltaTime;
            gameTimeImage.fillAmount = timer / GameTime;

            if (timer > GameTime) GameOver();
        }

        private void GameOver()
        {
            GameEnded = true;
            // Save to Recipe

            gameTimeImage.fillAmount = 1f;

            if (successTap == 0)
            {
                OnCookingFailed?.Invoke();

                CookingManager.Instance.CookingFailed();
            }
            else
            {
                OnCookingSuccess?.Invoke();

                CookingManager.Instance.CookingDone(CookingIndicator.FinalResult);
            }
        }

        public void TapSuccess(int num)
        {
            if (num == lastTapNum + 1)
            {
                successTap++;

                OnCookingHit?.Invoke();
            }
            else
            {
                Debug.Log("Number not ascending : " + lastTapNum);
                missedTap++;

                OnCookingMissed?.Invoke();
            }
            lastTapNum = num;

            float accuracy = (float)successTap / (successTap + missedTap);
            accuracyText.text = Mathf.RoundToInt(accuracy * 100f).ToString() + "%";
            // Update according current total Target executed (Start: Perfect > Good > Bad)
            CookingIndicator.SetIndicatorUI(accuracy);
        }

        public void TapMissed(int num)
        {
            missedTap++;
            lastTapNum = num;

            float accuracy = (float)successTap / (successTap + missedTap);
            accuracyText.text = Mathf.RoundToInt(accuracy * 100f).ToString() + "%";
            // Update according current total Target executed (Start: Perfect > Good > Bad)
            CookingIndicator.SetIndicatorUI(accuracy);

            OnCookingMissed?.Invoke();
        }
    }
}