using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEngageState : NPCState
{

    private Transform centerObject; // The object to circle around
    private float radius = 6f; // The radius of the circle
    Vector3 npcPos;

    private float angle; // Current angle of rotation
    private float randomSpeed;
    private bool isClockwise; // Variable to determine the rotation direction
    public EnemyEngageState(NPC.NPC npc, NPCStateMachine npcStateMachine) : base(npc, npcStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        npc.IsEngaging = true;
        isClockwise = Random.Range(0, 2) == 0;
        randomSpeed = Random.Range(0.4f, 1.3f) * (isClockwise ? 1 : -1); ;
        angle = Random.Range(0f, 360f);
        radius = Random.Range(6f, 8f);
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.IsEngaging = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        centerObject = GameManager.Instance.PlayerTransform;
        npcPos = npc.transform.position;
        angle += randomSpeed * Time.deltaTime;

        // Update the angle based on a random speed
        

        // Calculate the position on the circle using polar coordinates
        float x = centerObject.position.x + Mathf.Cos(angle) * radius;
        float z = centerObject.position.z + Mathf.Sin(angle) * radius;

        NavMeshHit hit;
        NavMesh.SamplePosition(new Vector3(x, npc.transform.position.y, z), out hit, Mathf.Infinity, NavMesh.AllAreas);

        npc.Agent.SetDestination(hit.position);

        if (Vector3.SqrMagnitude(centerObject.position - npcPos) > npc.CombatEngageDistance)
        {
            npc.StateMachine.ChangeState(new EnemyMoveState(npc.GetComponent<NPC.NPC>(), npc.StateMachine));
        }

        RotateToTarget(15f);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void RotateToTarget(float rotationSpeed)
    {
        // Calculate the direction from this GameObject to the target
        Vector3 direction = npc.CurrentEnemies.transform.position - npcPos;
        direction.y = 0;

        // Create a rotation that looks in the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate towards the target rotation
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
