using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [Serializable]
    public struct DamageInfo
    {
        public DamageType DamageType;
        public int Value;
    }

    [RequireComponent(typeof(Collider))]
    public class Weapon : MonoBehaviour
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
