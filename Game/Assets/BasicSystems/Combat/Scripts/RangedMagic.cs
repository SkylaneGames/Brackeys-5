using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Collider))]
    public class RangedMagic : Weapon
    {
        public float Speed = 5;

        void FixedUpdate()
        {
            transform.Translate(transform.forward * Time.deltaTime * Speed, Space.World);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.GetComponent<CombatSystem>() != Caller)
            {
                Destroy(gameObject);
            }
        }
    }
}
