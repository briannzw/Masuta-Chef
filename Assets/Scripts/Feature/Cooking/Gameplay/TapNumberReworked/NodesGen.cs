using AYellowpaper.SerializedCollections;
using Cooking.Gameplay;
using Cooking.Gameplay.TapNumber;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Pattern
{
    public class NodesGen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform nodeParent;
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private CookingNumberManager manager;

        [Header("Nodes Settings")]
        [SerializeField] private SerializedDictionary<LineLength, CookingDifficultyFrequency> nodesCount;
        [SerializeField] private float nodeTiming = 0.5f;

        private int globalCount = 0;

        public IEnumerator GenerateNodes(LineLength len, Vector2 from, Vector2 to, float duration)
        {
            float lerpValue = 0.2f;
            int randomCount = Random.Range(Mathf.RoundToInt(nodesCount[len].Min), Mathf.RoundToInt(nodesCount[len].Max) + 1);
            int currentCount = 1;
            while (lerpValue <= 1f)
            {
                var nodeGO = Instantiate(nodePrefab, Lerp(from, to, lerpValue), Quaternion.identity, nodeParent);
                var rect = nodeGO.GetComponent<RectTransform>();
                rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y, 0f);
                rect.SetAsFirstSibling();
                var node = nodeGO.GetComponent<CookingNumberController>();
                node.Duration = (duration / randomCount) + nodeTiming;
                node.Manager = manager;
                node.internalCount = globalCount++;
                node.NumberText.text = currentCount++.ToString();
                yield return new WaitForSeconds(duration / randomCount);
                lerpValue += .8f / randomCount;
            }
        }

        private Vector2 Lerp(Vector2 start, Vector2 end, float percent)
        {
            return (start + percent * (end - start));
        }
    }
}