using AYellowpaper.SerializedCollections;
using Cooking.Gameplay;
using System.Collections;
using UnityEngine;

namespace Pattern
{
    public class PatternGen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LineGen lineGen;

        [Header("Parameters")]
        [SerializeField] private float playTime = 60f;
        [SerializeField] private SerializedDictionary<LineLength, CookingDifficultyFrequency> linesTime;

        [Header("Pattern")]
        [SerializeField] private int maxRepeat = 3;
        [SerializeField] private float waitTime = .5f;

        private float playTimer;

        // Repeat
        private LineLength prevLen;
        private int repeated = 0;

        private void Start()
        {
            StartCoroutine(GeneratePattern());
        }

        private void Update()
        {
            if (playTimer < playTime) playTimer += Time.deltaTime;
        }

        private IEnumerator GeneratePattern()
        {
            while (playTimer < playTime)
            {
                // FEED PATTERN HERE
                var len = (LineLength)Random.Range(0, System.Enum.GetValues(typeof(LineLength)).Length);

                if (prevLen == len) repeated++;

                if (repeated == maxRepeat)
                {
                    while (len == prevLen) len = (LineLength)Random.Range(0, System.Enum.GetValues(typeof(LineLength)).Length);
                    repeated = 0;
                }

                prevLen = len;

                float randomDuration = Random.Range(linesTime[len].Min, linesTime[len].Max);
                lineGen.GenerateLine(len, randomDuration);
                yield return new WaitForSeconds(randomDuration + waitTime);
            }
        }
    }
}