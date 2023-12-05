using AYellowpaper.SerializedCollections;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace Cooking
{
    public enum CookingResult { Bad, Good, Perfect }
}

namespace Cooking.Gameplay.UI
{

    public class CookingIndicator : MonoBehaviour
    {
        [Header("References")]
        public Image IndicatorImage;
        [SerializeField] private UnityEngine.UI.Slider leftBarSlider;

        [Header("Accuracy Values")]
        [ReadOnly] public SerializedDictionary<CookingResult, float> AccuracyPercentages = new();

        [Header("Parameter")]
        public SerializedDictionary<CookingResult, Sprite> AccuracySprites = new();
        [ReadOnly] public CookingResult FinalResult;

        // Optimization
        private CookingResult prevResult;

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

            leftBarSlider.value = accuracy;
            // Optimization for UI drawcall
            if (FinalResult == prevResult) return;
            prevResult = FinalResult;

            IndicatorImage.sprite = AccuracySprites[FinalResult];
        }
    }
}