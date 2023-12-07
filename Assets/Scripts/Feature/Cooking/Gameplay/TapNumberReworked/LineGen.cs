using System.Collections;
using UnityEngine;

public class LineGen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform playPanel;

    [Header("Parameters")]
    [SerializeField] private float playTime = 60f;
    [SerializeField] private float lineTime;

    [Header("Line Settings")]
    [SerializeField] private float minPointDistance;
    [SerializeField] private float maxPointDistance;

    private float playTimer;
    private Vector3 prevPos;
    private Vector3 pos;

    private void Start()
    {
        // Starting Pos
        prevPos = new Vector3(Random.Range(-playPanel.rect.width / 2, playPanel.rect.width / 2), Random.Range(-playPanel.rect.height / 2, playPanel.rect.height / 2), 0);

        StartCoroutine(GenerateLine());
    }

    private void Update()
    {
        if(playTimer < playTime) playTimer += Time.deltaTime;
    }

    private IEnumerator GenerateLine()
    {
        while (playTimer < playTime)
        {
            pos = new Vector3(Random.Range(-playPanel.rect.width / 2, playPanel.rect.width / 2), Random.Range(-playPanel.rect.height / 2, playPanel.rect.height / 2), 0);

            Debug.Log(Vector2.Distance(prevPos, pos).ToString());
            yield return new WaitForSeconds(lineTime);

            prevPos = pos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(playPanel.TransformPoint(pos), 5f);

        Gizmos.DrawLine(playPanel.TransformPoint(prevPos), playPanel.TransformPoint(pos));
    }
}
