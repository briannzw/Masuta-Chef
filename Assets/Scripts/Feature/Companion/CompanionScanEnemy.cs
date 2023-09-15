using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionScanEnemy : MonoBehaviour
{
    [SerializeField] private float detectEnemyRadius = 10f; //The detect radius is from the player position
    private Companion companion;
    public static GameObject currentNearestEnemy;
   
    private void Awake()
    {
        companion = GetComponent<Companion>();
    }
    private void Update()
    {
        CheckForEnemy();
    }
    private void CheckForEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(GameManager.Instance.PlayerGameObject.transform.position, detectEnemyRadius);
        float nearestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // Check if this is the nearest enemy inside the radius
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    currentNearestEnemy = collider.gameObject;
                    companion.UpdateNearestEnemy(currentNearestEnemy);

                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GameManager.Instance.PlayerGameObject.transform.position, detectEnemyRadius);
    }
}
