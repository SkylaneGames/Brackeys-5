using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Collider))]
    public class AOEMagic : Weapon
    {
        public float lifetime = 1;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            lifetime -= Time.deltaTime;

            if (lifetime <= 0)
            {
                Destroy(this);
            }
        }
    }
}
