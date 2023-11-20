using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cooking.Gameplay.Slider
{
    using AYellowpaper.SerializedCollections;
    using Gameplay.UI;

    public class CookingBarCursorController : CookingGameplay
    {
        [Header("References")]
        public CookingBarController CookingBarController;
        public RectTransform CursorTransform;

        [Header("Indicator UI")]
        public CookingIndicator CookingIndicator;
        public SerializedDictionary<CookingResult, float> AccuracyPercentages = new();

        [Header("Parameters")]
        [Range(0, 1)] public float SpeedScale = .4f;

        private PlayerAction cookingControls;
        private float direction = 0;

        private float xPos = 0f;

        // Timer
        [Header("Timer")]
        public float TimeoutTime = 5f;
        public float GameTime = 60f;

        private float timeoutTimer = 0f;
        private float stayTime = 0f;
        private float gameTimer = 0f;


        #region Actions
        private int prevState;
        #endregion

        private void Awake()
        {
            CookingIndicator.AccuracyPercentages = AccuracyPercentages;
            // Set to PERFECT
            CookingIndicator.SetIndicatorUI(1f);
        }

        private void Start()
        {
            cookingControls = InputManager.PlayerAction;

            // Could be source of input bug on other scenes
            InputManager.ToggleActionMap(cookingControls.Cooking);
            RegisterInputCallbacks();
        }

        private void OnEnable()
        {
            RegisterInputCallbacks();
        }

        private void OnDisable()
        {
            UnregisterInputCallbacks();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<float>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            direction = 0f;
        }

        private void Update()
        {
            if (!cookingControls.Cooking.enabled) return;

            gameTimer += Time.deltaTime;

            // Update UI (unoptimized)
            CookingIndicator.SetIndicatorUI(stayTime / gameTimer);

            // Time Out
            if (timeoutTimer >= TimeoutTime)
            {
                Debug.Log("Timeout!");
                OnCookingFailed?.Invoke();
                GameOver();
                return;
            }

            // Game Time Ends
            if (gameTimer > GameTime)
            {
                OnCookingSuccess?.Invoke();
                GameOver();
            }

            xPos = Mathf.Clamp(CursorTransform.anchoredPosition.x + direction * SpeedScale * CookingBarController.MainSize * Time.deltaTime, 0f, CookingBarController.MainSize);
            CursorTransform.anchoredPosition = new Vector2(xPos, CursorTransform.anchoredPosition.y);

            if (!CookingBarController.IsInsideBar(xPos - CookingBarController.MainSize / 2))
            {
                timeoutTimer += Time.deltaTime;

                // To ensure only called once
                if (prevState != 0)
                {
                    prevState = 0;
                    OnCookingMissed?.Invoke();
                }
            }
            else
            {
                stayTime += Time.deltaTime;
                timeoutTimer = 0f;

                // To ensure only called once
                if(prevState != 1)
                {
                    prevState = 1;
                    OnCookingHit?.Invoke();
                }
            }
        }

        private void GameOver()
        {
            cookingControls.Cooking.Disable();
            CookingBarController.GameOver();
            // Show Result
            // Toggle to Panel Input (?)
            Debug.Log(CookingIndicator.FinalResult.ToString());

            // Save to Recipe
            CookingManager.Instance.CookingDone(CookingIndicator.FinalResult);
        }

        #region Callbacks
        private void RegisterInputCallbacks()
        {
            if (cookingControls == null) return;

            cookingControls.Cooking.Move.performed += OnMove;
            cookingControls.Cooking.Move.canceled += OnMoveCanceled;
        }

        private void UnregisterInputCallbacks()
        {
            if (cookingControls == null) return;

            cookingControls.Cooking.Move.performed -= OnMove;
            cookingControls.Cooking.Move.canceled -= OnMoveCanceled;
        }
        #endregion

        #region
        private void OnGUI()
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(10, 10, 400, 50), "Game Timer : " + gameTimer);
            GUI.Label(new Rect(10, 60, 400, 50), "Timeout Timer : " + timeoutTimer);
            GUI.Label(new Rect(10, 110, 400, 50), "Stay Percentage : " + stayTime / gameTimer * 100 + "%");
        }
        #endregion
    }
}