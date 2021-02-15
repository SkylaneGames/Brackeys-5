using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public delegate void RageChanged(float normalisedValue);

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(HealthSystem))]
    public class CombatSystem : MonoBehaviour
    {
        public event RageChanged RageChanged;

        [SerializeField]
        [Range(0, 1)]
        private float rage = 0;

        public float Rage
        {
            get { return rage; }
            set
            {
                rage = Mathf.Clamp01(value);
                RageChanged?.Invoke(rage);
            }
        }

        private HealthSystem _healthSystem;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
        }

        public void TakeDamage(IEnumerable<DamageInfo> hits)
        {
            foreach (var damage in hits)
            {
                var hitDamage = damage;

                // Scale physical attacks based on rage.
                if (damage.DamageType == DamageType.Physical)
                {
                    hitDamage.Value += Mathf.RoundToInt(hitDamage.Value * rage);
                }

                _healthSystem.Damage(hitDamage);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            var weapon = collider.GetComponent<PhysicalWeapon>();

            if (weapon != null)
            {
                TakeDamage(weapon.Damage);
            }
        }
    }
}
