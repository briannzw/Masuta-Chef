using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBall : MonoBehaviour
{
    public enum TargetType
    {
        Player,
        Enemy
    }
    public TargetType selectedTarget;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float shootForce = 10f;
    public float ShootInterval = 2f;

    public void Shoot()
    {
        GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation);
        //ball.GetComponent<Ball>().selectedTarget = selectedTarget;
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        // Calculate the direction from this object to the target
        Vector3 shootDirection = (GetComponent<NPC.NPC>().TargetPosition - transform.position).normalized;

        // Apply force to the ball in the calculated direction
        rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
    }
}
