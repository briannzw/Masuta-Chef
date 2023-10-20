using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class SaberVFXController : MonoBehaviour
    {
        [SerializeField] private GameObject shockwave;

        public void StartVFX()
        {
            shockwave.SetActive(true);
        }
    }
}