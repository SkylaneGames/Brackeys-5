using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(Collider))]
    public class PhysicalWeapon : MonoBehaviour
    {
        private Collider _collider;

        public List<DamageInfo> Damage;

        void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        // Used to enable the collider for the duration of an attack (tie this into the attack animation (directly or via combat system))
        public void SetDamageActive(bool active)
        {
            _collider.enabled = active;
        }
    }
}
