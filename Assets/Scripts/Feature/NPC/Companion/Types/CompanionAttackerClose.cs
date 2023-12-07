using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC.Companion
{
    public class CompanionAttackerClose : Companion
    {
        [SerializeField] float wanderRadius;
        [SerializeField] float wanderInterval;
        public override float WanderRadius => wanderRadius;
        public override float WanderInterval => wanderInterval;
        [SerializeField] float rotationSpeed = 5.0f;
        protected override void Update()
        {
            base.Update();
            DetectTarget();
        }
    }
}
