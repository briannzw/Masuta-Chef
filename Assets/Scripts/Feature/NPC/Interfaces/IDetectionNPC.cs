using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetectionNPC
{
    public float DetectionRadius { get; set; }
    public string TargetTag { get; set; }
    public void DetectTarget();
}
