using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cooking.Gameplay.Circular {
    public class CookingCircularBarController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform deadBarFill;
        [SerializeField] private RectTransform indicator;

        [Header("Parameters")]
        [SerializeField, Range(0.5f, 1f)] private float deadBarScale;
        [SerializeField, Range(0f, 1f)] private float indicatorToSafeBar;

        [Header("Difficulty")]
        [SerializeField] private float GameTime = 5f;
        [SerializeField] private CookingDifficulty currentDifficulty;
        [SerializeField] private SerializedDictionary<CookingDifficulty, float> dropRatePerSeconds;

        private float timer = 0f;

        private void Start()
        {
            deadBarFill.anchorMax = new Vector2(deadBarScale, deadBarFill.anchorMax.y);
            indicator.anchorMin = new Vector2(deadBarScale + (1 - deadBarScale) * indicatorToSafeBar, .5f);
            indicator.anchorMax = indicator.anchorMin;
            indicator.anchoredPosition = new Vector2(0f, indicator.anchoredPosition.y);
        }

        private void Update()
        {
            if (timer >= GameTime) return;
            if (indicator.anchorMin.x <= 0f) return;
            
            timer += Time.deltaTime;
            if (timer >= GameTime)
            {
                GameOver();
                return;
            }

            indicator.anchorMin = new Vector2(indicator.anchorMin.x - Time.deltaTime * dropRatePerSeconds[currentDifficulty], .5f);
            indicator.anchorMin = new Vector2(Mathf.Clamp(indicator.anchorMin.x, 0f, 1f), .5f);
            indicator.anchorMax = indicator.anchorMin;

            if (indicator.anchorMin.x == 0) GameOver();
        }

        private void GameOver()
        {
            if (indicator.anchorMin.x < 0.5f) Debug.Log("Result : BAD");
            else if (indicator.anchorMin.x < deadBarScale) Debug.Log("Result : GOOD");
            else Debug.Log("Result : PERFECT");
        }

        private void OnValidate()
        {
            if (!deadBarFill) return;
            deadBarFill.anchorMax = new Vector2(deadBarScale, deadBarFill.anchorMax.y);
            if (!indicator) return;
            indicator.anchorMin = new Vector2(deadBarScale + (1 - deadBarScale) * indicatorToSafeBar, .5f);
            indicator.anchorMax = indicator.anchorMin;
            indicator.anchoredPosition = new Vector2(0f, indicator.anchoredPosition.y);
        }

        #region GUI
        private void OnGUI()
        {
            GUI.skin.label.fontSize = 50;
            GUI.Label(new Rect(10, 10, 500, 200), "Time Left : " + (GameTime - timer));
        }
        #endregion
    }
}