using System.Collections.Generic;
using UnityEngine;

namespace Module.Detector
{
    public static class ColliderDetector
    {
        public static List<T> Find<T>(Vector3 axis, float radius, LayerMask targetMask, Vector3 forward = new Vector3(), float angle = 360) where T: class
        {
            List<T> objectsFound = new List<T>();

            Collider[] targetsInRadius = Physics.OverlapSphere(axis, radius, targetMask);

            foreach (Collider targetCollider in targetsInRadius)
            {
                Transform target = targetCollider.transform;
                Vector3 directionToTarget = (target.position - axis).normalized;
                if (Vector3.Angle(forward, directionToTarget) <= (angle / 2))
                {
                    if (typeof(T) == typeof(GameObject))
                    {
                        objectsFound.Add(target.gameObject as T);
                        continue;
                    }
                    if (typeof(T) == typeof(Transform))
                    {
                        objectsFound.Add(target as T);
                        continue;
                    }
                    if (target.GetComponent<T>() != null)
                    {
                        objectsFound.Add(target.GetComponent<T>());
                    }
                }
            }
            return objectsFound;
        }

        public static T FindNearest<T>(Vector3 axis, float radius, LayerMask targetMask, Vector3 forward = new Vector3(), float angle = 360) where T: class
        {
            T nearestObject = null;
            float nearestDistance = Mathf.Infinity;

            Collider[] targetsInRadius = Physics.OverlapSphere(axis, radius, targetMask);

            foreach (Collider targetCollider in targetsInRadius)
            {
                Transform target = targetCollider.transform;
                Vector3 directionToTarget = (target.position - axis).normalized;
                if (Vector3.Angle(forward, directionToTarget) <= (angle / 2))
                {
                    if ((axis - target.position).sqrMagnitude > nearestDistance) continue;
                    nearestDistance = (axis - target.position).sqrMagnitude;

                    if (typeof(T) == typeof(GameObject))
                    {
                        nearestObject = target.gameObject as T;
                        continue;
                    }
                    if (typeof(T) == typeof(Transform))
                    {
                        nearestObject = target as T;
                        continue;
                    }
                    if (target.GetComponent<T>() != null)
                    {
                        nearestObject = target.GetComponent<T>();
                    }
                }
            }
            return nearestObject;
        }
    }
}