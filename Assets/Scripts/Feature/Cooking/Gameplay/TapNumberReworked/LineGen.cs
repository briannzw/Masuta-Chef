using AYellowpaper.SerializedCollections;
using Cooking.Gameplay;
using System.Collections;
using UnityEngine;

namespace Pattern {
    public enum LineLength { Short, Middle, Long }

    public class LineGen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private NodesGen nodesGen;

        [Header("UI References")]
        [SerializeField] private RectTransform playPanel;

        [Header("Line Settings")]
        [SerializeField] private SerializedDictionary<LineLength, CookingDifficultyFrequency> pointDistances;

        private Vector3 prevPos;
        private Vector3 pos;

        private void Start()
        {
            // Starting Pos
            prevPos = new Vector3(Random.Range(-playPanel.rect.width / 2, playPanel.rect.width / 2), Random.Range(-playPanel.rect.height / 2, playPanel.rect.height / 2), 0);
        }

        public void GenerateLine(LineLength len, float duration, bool newPattern = false)
        {
            prevPos = (newPattern) ? new Vector3(Random.Range(-playPanel.rect.width / 2, playPanel.rect.width / 2), Random.Range(-playPanel.rect.height / 2, playPanel.rect.height / 2), 0) : pos;

            float distance = 0f;
            while (distance > pointDistances[len].Max || distance < pointDistances[len].Min)
            {
                pos = new Vector3(Random.Range(-playPanel.rect.width / 2, playPanel.rect.width / 2), Random.Range(-playPanel.rect.height / 2, playPanel.rect.height / 2), 0);
                distance = Vector2.Distance(prevPos, pos);
            }

            StartCoroutine(nodesGen.GenerateNodes(len, playPanel.TransformPoint(prevPos), playPanel.TransformPoint(pos), duration));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(playPanel.TransformPoint(pos), 5f);

            Gizmos.DrawLine(playPanel.TransformPoint(prevPos), playPanel.TransformPoint(pos));
        }
    }
}