using System;
using System.Collections;
using UnityEngine;

namespace Cooking.Gameplay.TapNumber
{
    using AYellowpaper.SerializedCollections;
    using static UnityEngine.Rendering.DebugUI;
    using Random = UnityEngine.Random;

    public class CookingNumberSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CookingNumberManager manager;
        [SerializeField] private RectTransform canvasTransform;
        [SerializeField] private GameObject numberPrefab;

        [Header("Parameters")]
        [SerializeField] private SerializedDictionary<CookingDifficulty, CookingNumberMinMax> minMaxSpawnFrequency;

        private int spawnedCount = 0;

        private Vector3 pos;
        private int loopCount = 0;
        private int maxLoopIteration = 100;

        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                Vector3 randomPos = CheckPos();
                GameObject go = Instantiate(numberPrefab, canvasTransform);
                go.transform.localPosition = randomPos;
                spawnedCount++;
                CookingNumberController controller = go.GetComponent<CookingNumberController>();
                controller.Duration = Random.Range(manager.NumberDuration[manager.CurrentDifficulty].MinTime, manager.NumberDuration[manager.CurrentDifficulty].MaxTime);
                controller.Manager = manager;
                controller.NumberText.text = spawnedCount.ToString();
                if (spawnedCount >= manager.TargetCount[manager.CurrentDifficulty]) yield break;
                yield return new WaitForSeconds(Random.Range(minMaxSpawnFrequency[manager.CurrentDifficulty].MinTime, minMaxSpawnFrequency[manager.CurrentDifficulty].MaxTime));
            }
        }

        private Vector3 CheckPos()
        {
            loopCount = 0;
            while (true)
            {
                loopCount++;
                pos = new Vector3(Random.Range(-canvasTransform.rect.width / 2, canvasTransform.rect.width / 2), Random.Range(-canvasTransform.rect.height / 2, canvasTransform.rect.height / 2), 0);
                // If there's no other number there
                if (!Physics2D.OverlapBox(canvasTransform.TransformPoint(pos), numberPrefab.GetComponent<RectTransform>().rect.size, 0f, LayerMask.GetMask("UI")))
                    break;
                // Prevent Infinite Loop
                if (loopCount > maxLoopIteration) break;
            }

            return pos;
        }
    }
}