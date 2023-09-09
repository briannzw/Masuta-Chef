using System.Collections;
using UnityEngine;

namespace Cooking.Gameplay.Slider
{
    public class CookingBarController : MonoBehaviour
    {
        [Header("References")]
        public RectTransform MainArea;
        public RectTransform BarArea;

        [Header("Parameters")]
        [Range(0f, 1f)] public float BarScale;

        [Header("Difficulty")]
        [Range(0f, 1f)] public float BarSpeed;
        public bool DirectionChange;
        public float DirChangeMinFrequency = 2;
        public float DirChangeMaxFrequency = 5;

        private float dirTimer;
        private float dirRandomTime;

        [ReadOnly] public float MainSize;
        private float barHalf;

        private float currentOffset = 0f;

        private void Start()
        {
            MainSize = MainArea.rect.width;

            SetArea();

            dirRandomTime = Random.Range(DirChangeMinFrequency, DirChangeMaxFrequency);
            // Random direction
            StartCoroutine(MoveBar(Random.Range(0, 2) * 2 - 1));
        }

        private void SetArea(float offset = 0f)
        {
            // Condition : bar does not exceed the main area.
            barHalf = MainSize / 2 * (1 - BarScale);
            BarArea.offsetMin = new Vector2(barHalf + offset, BarArea.offsetMin.y);    // Set Left
            BarArea.offsetMax = new Vector2(-barHalf + offset, BarArea.offsetMax.y);   // Set Rights
        }

        private IEnumerator MoveBar(int direction)
        {
            while (true)
            {
                // If enable Direction Change
                if (DirectionChange)
                {
                    dirTimer += Time.deltaTime;
                    if (dirTimer > dirRandomTime)
                    {
                        direction *= -1;
                        dirTimer = 0f;
                        dirRandomTime = Random.Range(DirChangeMinFrequency, DirChangeMaxFrequency);
                    }
                }

                currentOffset += direction * (BarSpeed * MainSize) * Time.deltaTime;
                // Snap to Left
                if (barHalf + currentOffset <= 0f)
                {
                    currentOffset = -barHalf;
                    break;
                }
                // Snap to Right
                if(-barHalf + currentOffset >= 0f)
                {
                    currentOffset = barHalf;
                    break;
                }
                SetArea(currentOffset);
                yield return null;
            }

            SetArea(currentOffset);

            // Bar reached edge, proceed to change direction
            StartCoroutine(MoveBar(-direction));
        }

        public void GameOver()
        {
            StopAllCoroutines();
        }

        // Check cursor pos
        public bool IsInsideBar(float x)
        {
            return (x >= currentOffset - MainSize * BarScale / 2) && (x <= currentOffset + MainSize * BarScale / 2);
        }
    }
}