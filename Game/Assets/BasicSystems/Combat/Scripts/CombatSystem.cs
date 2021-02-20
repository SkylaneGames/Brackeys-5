using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Combat
{
    public delegate void RageChanged(float normalisedValue);
    public delegate void CharacterAttacked(CharacterController initiatingCharacter);

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(HealthSystem))]
    public class CombatSystem : MonoBehaviour
    {
        public event CharacterAttacked CharacterAttacked;
        public event RageChanged RageChanged;

        [SerializeField]
        private float attackDuration = 1f;

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

        [SerializeField]
        [Range(0, 1)]
        [Tooltip("The amount of rage the character gains per point of damage (Rage ranges from 0 to 1, so at 1, one point of damage would max out rage).")]
        private float rageIncreasePerDamge = 0.01f;

        public bool IsAttacking { get; private set; } = false;

        public bool CanAttack
        {
            get { return !IsAttacking && weapon.Damage.Count > 0; }
        }

        private HealthSystem _healthSystem;
        public HealthSystem HealthSystem { get { return _healthSystem; } }

        private Animator _animator;

        private Weapon weapon;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            if (_animator == null)
            {
                _animator = _animator ?? transform.parent.GetComponent<Animator>();
            }

            weapon = GetComponentInChildren<Weapon>();

            _healthSystem.CharacterHit += OnCharacterHit;
        }

        private void OnCharacterHit(int damage)
        {
            float dRage = damage * rageIncreasePerDamge;
            Rage += dRage;
        }

        public void Attack()
        {
            if (CanAttack)
            {
                IsAttacking = true;
                StartCoroutine("EnableHitColliderAfterDuration");
                StartCoroutine("DisableHitColliderAfterDuration");
                StartCoroutine("SetAttackedFinishedAfterDuration");
                _animator.SetTrigger("Attack");
            }
        }

        private IEnumerator EnableHitColliderAfterDuration()
        {
            yield return new WaitForSeconds(0.5f);
            SetWeaponActive(true);
        }

        private IEnumerator DisableHitColliderAfterDuration()
        {
            yield return new WaitForSeconds(0.6f);
            SetWeaponActive(false);
        }

        private IEnumerator SetAttackedFinishedAfterDuration()
        {
            yield return new WaitForSeconds(attackDuration);
            IsAttacking = false;
        }

        private void SetWeaponActive(bool active)
        {
            weapon.SetDamageActive(active);
        }

        public void TakeDamage(IEnumerable<DamageInfo> hits)
        {
            foreach (var damage in hits)
            {
                var hitDamage = damage;

                // // Scale physical attacks based on rage (need to get the attacking combat system's rage level).
                // if (damage.DamageType == DamageType.Physical)
                // {
                //     hitDamage.Value += Mathf.RoundToInt(hitDamage.Value);
                // }

                _healthSystem.Damage(hitDamage);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            var weapon = collider.GetComponent<Weapon>();

            if (weapon != null && weapon.Caller != this)
            {
                var physicalDamage = weapon.Damage.Any(p => p.DamageType == DamageType.Physical);
                var magicDamage = weapon.Damage.Any(p => p.DamageType == DamageType.Magic);

                if ((physicalDamage && !HealthSystem.ResistantToPhysicalDamage) || (magicDamage && !HealthSystem.ResistantToMagicalDamage))
                {
                    var attacker = collider.GetComponentInParent<CharacterController>();
                    Debug.Log($"[{name}] hit by '{collider.name}'");
                    TakeDamage(weapon.Damage);
                    CharacterAttacked?.Invoke(attacker);
                }

            }
        }
    }
}
