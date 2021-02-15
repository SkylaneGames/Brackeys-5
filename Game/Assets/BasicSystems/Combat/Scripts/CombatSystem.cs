using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CombatSystem : MonoBehaviour
    {
        public event Action CombatFinished;

        public Weapon Unarmed;
        public Weapon EquipedWeapon = null;

        [Range(0,1)]
        public float Rage = 0;

        public void OnCombatFinished()
        {
            CombatFinished?.Invoke();
        }

        public void Attack(LifeSystem enemy)
        {
            var weaponDamage = EquipedWeapon?.Damage.ToArray() ?? Unarmed.Damage.ToArray();

            foreach (var damage in weaponDamage)
            {
                enemy.Damage(damage);
            }
        }
    }
}
