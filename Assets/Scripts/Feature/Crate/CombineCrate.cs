using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineCrate : MonoBehaviour
{
    private List<CombineCrate> collidingCrates = new List<CombineCrate>();
    private Renderer crateRenderer;

    public float CombineSpeed;
    public float Distance;
    public string areaColliderObjectName = "AreaCollider";

    private GameObject areaColliderObject;

    public enum CubeState { Gray, Red, Blue, Yellow };
    private CubeState currentState = CubeState.Gray;

    private void Start()
    {
        crateRenderer = GetComponent<Renderer>();

        // Cari objek area collider berdasarkan nama saat permainan dimulai
        areaColliderObject = GameObject.Find(areaColliderObjectName);
        if (areaColliderObject == null)
        {
            Debug.LogError("Object with name '" + areaColliderObjectName + "' not found.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter: " + collision.gameObject.tag);

        if (IsInArea() && collision.gameObject.CompareTag("crate"))
        {
            CombineCrate otherCrate = collision.gameObject.GetComponent<CombineCrate>();

            if (otherCrate.currentState != CubeState.Gray)
            {
                Debug.Log("Other crate is not Gray.");
                return;
            }

            collidingCrates.Add(otherCrate);

            switch (currentState)
            {
                case CubeState.Gray:
                    SetCombinedObjectColor(Color.red);
                    currentState = CubeState.Red;

                    Debug.Log("Changing to Red");

                    Destroy(collision.gameObject);
                    break;
                case CubeState.Red:
                    SetCombinedObjectColor(Color.blue);
                    currentState = CubeState.Blue;

                    Debug.Log("Changing to Blue");

                    Destroy(collision.gameObject);
                    break;
                case CubeState.Blue:
                    SetCombinedObjectColor(Color.yellow);
                    currentState = CubeState.Yellow;

                    Debug.Log("Changing to Yellow");

                    Destroy(collision.gameObject);
                    break;
                case CubeState.Yellow:
                    // Tidak ada perubahan jika sudah biru
                    break;
            }

            Debug.Log("Current State: " + currentState);

            if (currentState != CubeState.Gray)
            {
                collidingCrates[0].collidingCrates.Remove(this);
                collidingCrates.RemoveAt(0);
            }
        }
    }


    private void FixedUpdate()
    {
        if (IsInArea())
        {
            MoveTowards();
        }
    }

    private bool IsInArea()
    {
        if (areaColliderObject == null)
        {
            return true;
        }

        Collider collider = areaColliderObject.GetComponent<Collider>();

        if (collider == null)
        {
            Debug.LogError("Collider component not found on " + areaColliderObject.name);
            return false;
        }

        // Periksa apakah objek berada di dalam collider area.
        return collider.bounds.Contains(transform.position);
    }

    public void MoveTowards()
    {
        if (collidingCrates.Count > 0)
        {
            Vector3 targetPosition = Vector3.zero;

            foreach (CombineCrate crate in collidingCrates)
            {
                targetPosition += crate.transform.position;
            }

            targetPosition /= collidingCrates.Count;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, CombineSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < Distance)
            {
                foreach (CombineCrate crate in collidingCrates)
                {
                    crate.collidingCrates.Clear();
                }
                collidingCrates.Clear();
            }
        }
    }

    private void SetCombinedObjectColor(Color color)
    {
        crateRenderer.material.color = color;
        foreach (CombineCrate crate in collidingCrates)
        {
            crate.crateRenderer.material.color = color;
        }

    }

    // biar bisa akses currstate
    public CubeState GetCurrentState()
    {
        return currentState;
    }
}