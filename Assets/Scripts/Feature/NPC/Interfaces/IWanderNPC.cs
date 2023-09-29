using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWanderNPC
{
    float WanderRadius { get; set; }
    float WanderInterval { get; set; }

    public void Wander();
}
