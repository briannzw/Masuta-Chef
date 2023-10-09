using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] float rotationSpeed = 5.0f;
    private new void Update()
    {
        base.Update();
        RotateToTarget(rotationSpeed);
    }
    
}
