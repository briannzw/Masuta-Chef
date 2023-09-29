using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] float rotationSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        // Calculate the direction from this GameObject to the target
        Vector3 direction = TargetPosition - transform.position;

        // Create a rotation that looks in the calculated direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
