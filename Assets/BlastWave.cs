using Character.Hit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;
using Module.Detector;

public class BlastWave : MonoBehaviour
{
    public int pointsCount;
    public float maxRadius;
    public float speed;
    public float startWidth;

    private LineRenderer lineRenderer;
    //[SerializeField] private AOEController controller;
    //private List<Character.Character> characterInArea = new List<Character.Character>();
    //[SerializeField] private LayerMask targetMask;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = pointsCount + 1;
    }

    private void OnEnable()
    {
        StartCoroutine(Blast());
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        StartCoroutine(Blast());
    //        Damage(maxRadius);
    //    }
    //}

    private IEnumerator Blast()
    {
        float currentRadius = 0f;

        while (currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
            yield return null;
        }
    }

    //private void Damage(float radius)
    //{
    //    characterInArea = ColliderDetector.Find<Character.Character>(transform.position, radius, targetMask);
    //    foreach (var chara in characterInArea.ToArray())
    //    {
    //        controller.Hit(chara);
    //    }
    //}

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / pointsCount;

        for (int i = 0; i <= pointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }
}
