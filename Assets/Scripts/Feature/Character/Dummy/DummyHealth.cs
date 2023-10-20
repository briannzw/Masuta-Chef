using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Dummy
{
    public class DummyHealth : MonoBehaviour
    {
        public float health = 100000;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Hit()
        {
            health -= 10;
        }
    }
}
