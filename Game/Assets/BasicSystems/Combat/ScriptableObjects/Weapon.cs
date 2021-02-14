using UnityEngine;
using System.Collections.Generic;

namespace Combat
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Equipment/Weapon")]
    public class Weapon : Equipment
    {
        public WeaponType WeaponType;

        public List<DamageInfo> Damage;
    }

    public enum WeaponType
    {
        OneHanded,
        TwoHanded,
        Staff,
        Bow,
        Unarmed
    }
}