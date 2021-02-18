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
        public HealthSystem HealthSystem { get { return _healthSystem; } }

        private Animator _animator;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            if (_animator == null)
            {
                _animator = _animator ?? transform.parent.GetComponent<Animator>();
            }
        }

        public void Attack()
        {
            _animator.SetTrigger("Attack");
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
            var weapon = collider.GetComponent<Weapon>();

            if (weapon != null && weapon.Caller != this)
            {
                Debug.Log($"[{name}] Weapon hit, damage: {weapon.Damage}");
                TakeDamage(weapon.Damage);
            }
        }
    }
}
