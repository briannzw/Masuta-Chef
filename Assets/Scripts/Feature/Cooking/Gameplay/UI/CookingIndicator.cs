using AYellowpaper.SerializedCollections;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking.Gameplay.UI
{
    public enum CookingResult { Bad, Good, Perfect }

    public class CookingIndicator : MonoBehaviour
    {
        [Header("References")]
        public Image IndicatorBackground;
        public TMP_Text IndicatorText;

        [Header("Accuracy Values")]
        [ReadOnly] public SerializedDictionary<CookingResult, float> AccuracyPercentages = new();

        [Header("Parameter")]
        public SerializedDictionary<CookingResult, Color> AccuracyColors = new();
        [ReadOnly] public CookingResult FinalResult;

        private void Start()
        {
            if (AccuracyPercentages.Count == 0) Debug.LogError("Please assign GoodPercentage and PerfectPercentage from script (in Awake()) that needed this indicator!");
        }

        public void SetIndicatorUI(float accuracy)
        {
            if (accuracy < AccuracyPercentages[CookingResult.Good])
            {
                FinalResult = CookingResult.Bad;
            }
            else if (accuracy < AccuracyPercentages[CookingResult.Perfect])
            {
                FinalResult = CookingResult.Good;
            }
            else
            {
                FinalResult = CookingResult.Perfect;
            }

            IndicatorBackground.color = AccuracyColors[FinalResult];
            IndicatorText.text = FinalResult.ToString().ToUpper();
        }
    }
}