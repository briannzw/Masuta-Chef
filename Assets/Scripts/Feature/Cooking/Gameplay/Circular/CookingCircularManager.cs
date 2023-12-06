using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using MyBox;

namespace Cooking.Gameplay.Circular
{
    using UI;

    public class CookingCircularManager : CookingGameplay
    {
        [Header("References")]
        [SerializeField] private CookingCircularController controller;
        [SerializeField] private Image arrowImage;

        [Header("Indicator UI")]
        public CookingIndicator CookingIndicator;
        public SerializedDictionary<CookingResult, float> AccuracyPercentages = new();

        [Header("Indicator Bar")]
        [SerializeField] private RectTransform indicatorBarImage;
        [SerializeField] private RectTransform indicatorBarCursor;
        [SerializeField] private float indicatorBarOffset = 10f;
        [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private ParticleSystem missedEffect;

        [Header("Game")]
        [SerializeField] private float gameTime = 60f;
        private float gameTimer = 0f;
        private bool gameEnded = false;

        [Header("Parameters")]
        [Range(0f, 1f), SerializeField] private float stirValue = 0.5f;
        [SerializeField] private CookingDifficultyFrequency stirNormal;
        [SerializeField] private float stirPower = 0.2f;

        [Header("Difficulty")]
        [SerializeField] private CookingDifficulty currentDifficulty;
        [SerializeField] private SerializedDictionary<CookingDifficulty, float> decreaseSpeed = new();
        [SerializeField] private SerializedDictionary<CookingDifficulty, float> dirChangeChance = new();
        [SerializeField] private SerializedDictionary<CookingDifficulty, CookingDifficultyFrequency> dirChangeFrequency = new();

        [Header("Animation")]
        [SerializeField] private int loop = 2;
        [SerializeField] private float duration = 0.75f;
        [SerializeField] private float fadeDuration = .15f;
        [SerializeField] private float zRotation = 180f;

        private Coroutine animCoroutine;

        private int currentDir = 0;
        private Vector3 currentAngle;

        private int prevState;

        private void Awake()
        {
            CookingIndicator.AccuracyPercentages = AccuracyPercentages;
            // Set to Default Stir Value
            CookingIndicator.SetIndicatorUI(stirValue);
        }

        private void Start()
        {
            StartCoroutine(SetDirection(Random.value < .5 ? 1 : -1));
            OnCookingHit += PlayHitEffect;
            OnCookingMissed += PlayMissedEffect;
        }

        private void Update()
        {
            if (gameEnded) return;

            // Constant Decrease
            if (stirValue > 0f) stirValue -= decreaseSpeed[currentDifficulty] * Time.deltaTime;
            else stirValue = 0f;

            float velocityAbs = Mathf.Abs(controller.CurrentVelocity);
            if (currentDir == -Mathf.Sign(controller.CurrentVelocity) && velocityAbs >= stirNormal.Min && velocityAbs <= stirNormal.Max)
            {
                stirValue += stirPower * Time.deltaTime;
                stirValue = Mathf.Clamp(stirValue, 0f, 1f);

                if (prevState != 1)
                {
                    OnCookingHit?.Invoke();
                    prevState = 1;
                }
            }
            else
            {
                if(prevState != 0)
                {
                    OnCookingMissed?.Invoke();
                    prevState = 0;
                }
            }

            UpdateIndicatorBar();
            CookingIndicator.SetIndicatorUI(stirValue);

            // Game
            gameTimer += Time.deltaTime;

            // Game Time Ends
            if (gameTimer > gameTime)
            {
                gameEnded = true;
                GameOver();
            }
        }

        private IEnumerator SetDirection(int dir)
        {
            while (true)
            {
                currentDir = dir;
                controller.CurrentDirection = dir;
                arrowImage.rectTransform.eulerAngles = new Vector3(0f, (dir == 1 ? 0f : 180f), 45f);

                if (animCoroutine != null) CancelAnimation();
                animCoroutine = StartCoroutine(AnimateArrow(arrowImage.rectTransform.eulerAngles.z - zRotation));

                yield return new WaitForSeconds(Random.Range(dirChangeFrequency[currentDifficulty].Min, dirChangeFrequency[currentDifficulty].Max));
                
                if (Random.Range(0, 100) < dirChangeChance[currentDifficulty] * 100) dir *= -1;
            }
        }

        private void GameOver()
        {
            controller.enabled = false;
            StopAllCoroutines();

            if (stirValue <= 0)
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

        private IEnumerator AnimateArrow(float zEndValue)
        {
            var startAngle = arrowImage.rectTransform.eulerAngles;
            for (int i = 0; i < loop; i++)
            {
                float time = 0f;

                // Reset Fade and Rotation
                currentAngle = startAngle;
                arrowImage.rectTransform.eulerAngles = startAngle;

                while (time < duration)
                {
                    // Fade In
                    if (time <= fadeDuration) arrowImage.SetAlpha(time / fadeDuration);

                    // Fade Out
                    if (duration - time <= fadeDuration) arrowImage.SetAlpha((duration - time) / fadeDuration);

                    // Rotate
                    currentAngle = new Vector3(currentAngle.x, currentAngle.y, Mathf.Lerp(currentAngle.z, zEndValue, Time.deltaTime));
                    arrowImage.rectTransform.eulerAngles = currentAngle;

                    time += Time.deltaTime;
                    yield return null;
                }

                arrowImage.SetAlpha(0f);

                // Delay between loops
                yield return new WaitForSeconds(.5f);
            }
        }

        private void CancelAnimation()
        {
            // Reset Fade and Rotation
            arrowImage.SetAlpha(0f);
            StopCoroutine(animCoroutine);
            animCoroutine = null;
        }

        private void UpdateIndicatorBar()
        {
            indicatorBarCursor.anchoredPosition = new Vector2(indicatorBarCursor.anchoredPosition.x, (stirValue - 0.5f) * (indicatorBarImage.sizeDelta.y - indicatorBarOffset * 2));
        }

        private void PlayHitEffect()
        {
            missedEffect.Stop();
            hitEffect.Play();
        }

        private void PlayMissedEffect()
        {
            hitEffect.Stop();
            missedEffect.Play();
        }

        private void OnGUI()
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(10, 10, 400, 50), "Stir Velocity: " + controller.CurrentVelocity);
            GUI.Label(new Rect(10, 60, 400, 50), "Game Timer : " + gameTimer);
        }
    }
}