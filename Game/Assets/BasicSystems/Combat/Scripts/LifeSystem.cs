using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Combat
{
    public class LifeSystem : MonoBehaviour
    {
        public event Action<int> PlayerDamaged;
        public event Action PlayerDied;

        public bool IsDead
        {
            get
            {
                return CurrentHealth <= 0;
            }
        }



        [Range(0, 1)]
        [Tooltip("The chance of armour reducing amount of damage taken. (Set to 1 to always remove armour rating from damage value)")]
        public float armourEffectiveness = 0.8f;

        [Range(0, 1)]
        [Tooltip("The chance armour will completely block an attack.")]
        public float armourBlockChance = 0.1f;

        public IEnumerable<DamageInfo> ArmourResistances;

        public int MaxHealth = 10;
        public int CurrentHealth { get; private set; }

        private Animator animator;

        void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            CurrentHealth = MaxHealth;
        }

        public void SetHealth(int hp)
        {
            CurrentHealth = Mathf.Clamp(hp, 0, MaxHealth);
            PlayerDamaged?.Invoke(CurrentHealth);
        }

        public void Damage(DamageInfo damage)
        {
            if (damage.Value < 0)
            {
                Debug.LogWarning("Damange less than 0.");
                return;
            }
            var value = ApplyArmourRating(damage.Value, damage.DamageType);
            // Debug.Log(damage.Value + ", " + value);

            CurrentHealth -= value;

            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }

            PlayerDamaged?.Invoke(CurrentHealth);

            if (CurrentHealth == 0)
            {
                OnDeath();
            }
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

        private void OnDeath()
        {

            animator.SetTrigger("Death");
            PlayerDied?.Invoke();
        }
    }
}
