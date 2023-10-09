using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] float rotationSpeed = 5.0f;

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        RotateToTarget(rotationSpeed);
    }
}
