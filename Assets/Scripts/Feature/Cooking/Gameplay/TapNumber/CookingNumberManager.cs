using System;
using UnityEngine;

namespace Cooking.Gameplay.TapNumber
{
    using AYellowpaper.SerializedCollections;
    using Cooking.Gameplay.UI;

    public class CookingNumberManager : CookingGameplay
    {
        [Header("Indicator UI")]
        public CookingIndicator CookingIndicator;
        public SerializedDictionary<CookingResult, float> AccuracyPercentages = new();

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

            if (timer > GameTime) GameOver();
        }

        private void GameOver()
        {
            GameEnded = true;
            // Save to Recipe

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

            // Update according current total Target executed (Start: Perfect > Good > Bad)
            CookingIndicator.SetIndicatorUI((float) successTap / (successTap + missedTap));
        }

        public void TapMissed(int num)
        {
            missedTap++;
            lastTapNum = num;

            // Update according current total Target executed (Start: Perfect > Good > Bad)
            CookingIndicator.SetIndicatorUI((float) successTap / (successTap + missedTap));

            OnCookingMissed?.Invoke();
        }

        #region GUI
        private void OnGUI()
        {
            GUI.skin.label.fontSize = 50;
            GUI.Label(new Rect(10, 10, 500, 200), "Missed Count : " + missedTap);
        }
        #endregion
    }
}