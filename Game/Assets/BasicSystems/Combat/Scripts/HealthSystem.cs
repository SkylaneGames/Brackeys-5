using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Combat
{
    public delegate void CharacterHit(int damage);
    public delegate void CharacterHealed(float amount);
    public delegate void HealthChanged(float normalisedValue);

    [RequireComponent(typeof(Collider))]
    public class HealthSystem : MonoBehaviour
    {
        public event HealthChanged HealthChanged;
        public event CharacterHealed CharacterHealed;
        public event CharacterHit CharacterHit;
        public event Action CharacterBlocked;
        public event Action CharacterKilled;

        public bool IsDead
        {
            get
            {
                return CurrentHealth == 0;
            }
        }

        public bool ResistantToPhysicalDamage = false;
        public bool ResistantToMagicalDamage = false;

        [Range(0,5)]
        public float HealthRegenOverTime = 0f;

        [Range(0, 1)]
        [Tooltip("The chance of armour reducing amount of damage taken. (Set to 1 to always remove armour rating from damage value)")]
        public float armourEffectiveness = 0.8f;

        [Range(0, 1)]
        [Tooltip("The chance armour will completely block an attack.")]
        public float armourBlockChance = 0.1f;

        public List<DamageInfo> ArmourResistances;

        [Range(0, 100)]
        public int MaxHealth = 10;

        private float currentHealth;
        public float CurrentHealth
        {
            get { return currentHealth; }
            private set
            {
                currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                HealthChanged?.Invoke((float)currentHealth / (float)MaxHealth);
            }
        }

        void Start()
        {
            CurrentHealth = MaxHealth;
        }

        void Update()
        {
            if (HealthRegenOverTime > 0)
            {
                CurrentHealth += HealthRegenOverTime * Time.deltaTime;
            }
        }

        public void Damage(DamageInfo damage)
        {
            if (ResistantToPhysicalDamage && damage.DamageType == DamageType.Physical)
            {
                return;
            }

            if (ResistantToMagicalDamage && damage.DamageType == DamageType.Magic)
            {
                return;
            }

            if (IsDead)
            {
                return;
            }

            if (damage.Value < 0)
            {
                Debug.LogWarning("Damange less than 0.");
                return;
            }
            var value = ApplyArmourRating(damage.Value, damage.DamageType);
            // Debug.Log(damage.Value + ", " + value);

            if (value == 0)
            {
                CharacterBlocked?.Invoke();
            }
            else
            {
                CurrentHealth -= value;

                CharacterHit?.Invoke(value);

                if (IsDead)
                {
                    CharacterKilled?.Invoke();
                }
            }
        }

        public void Kill()
        {
            CurrentHealth = 0;
            CharacterKilled?.Invoke();
        }

        public void Heal(int health)
        {
            if (CurrentHealth == MaxHealth)
            {
                Debug.Log($"[{transform.parent.name}] Already at max health.");
                return;
            }

            var amount = Mathf.Min(health, MaxHealth - CurrentHealth);

            CurrentHealth += amount;

            CharacterHealed?.Invoke(amount);
        }

        private int ApplyArmourRating(int damage, DamageType type)
        {
            DamageInfo? relevantRating = ArmourResistances.FirstOrDefault(p => p.DamageType == type);

            if (relevantRating.HasValue)
            {
                var armourEffective = UnityEngine.Random.Range(0.0f, 1.0f) < armourEffectiveness;
                var armourBlocked = UnityEngine.Random.Range(0.0f, 1.0f) < armourBlockChance;

                if (armourBlocked)
                {
                    damage = 0;
                }
                else if (armourEffective)
                {
                    damage = Mathf.Max(0, damage - relevantRating.Value.Value);
                }
            }

            return damage;
        }
    }
}
