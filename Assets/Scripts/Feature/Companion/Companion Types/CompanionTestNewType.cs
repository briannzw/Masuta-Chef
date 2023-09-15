using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionTestNewType : Companion
{

    private new void Awake()
    {
        base.Awake();
        StateMachine.Initialize(CompanionChasePlayerState);
    }

    private new void Update()
    {
        base.Update();
    }
}
