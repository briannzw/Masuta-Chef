using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionMovement : MonoBehaviour
{
    private Companion companion;
    private GameObject currentDestination;
    private Quaternion targetRotation;
    private void Awake()
    {
        companion = GetComponent<Companion>();
        companion.OnAgentDestinationChanged += HandleAgentDestinationChanged;
    }
    private void OnDisable()
    {
        companion.OnAgentDestinationChanged -= HandleAgentDestinationChanged;
    }

    void Update()
    {
        companion.Agent.SetDestination(currentDestination.transform.position);
        companion.Agent.speed = companion.MoveSpeed;
        if (!companion.Agent.isStopped)
        {
            UpdateRotation();
        }
    }
    private void UpdateRotation()
    {
        Vector3 velocity = companion.Agent.velocity;
        if (velocity.magnitude > 0)
        {
            targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * companion.Agent.angularSpeed);
        }
    }

    private void HandleAgentDestinationChanged(GameObject newAgentDestination)
    {
        // Do something with the new nearest enemy, such as updating AI behavior or targeting
        if (newAgentDestination != null)
        {
            Debug.Log("New Agent Destination is not null");
            Debug.Log(newAgentDestination.transform.position);
            // Set the current agent's destination to the position of the new destination
            currentDestination = newAgentDestination;
        }
    }
}
