using System;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
    public class Equipment : Item
    {
        public EquipmentSlot equipmentSlot;
        public GameObject gameObject;

        public override void Use(GameObject parent)
        {
            // var equipmentSystem = parent.GetComponent<EquipmentSystem>();

            // equipmentSystem.Equip(this);
        }
    }

    public enum EquipmentSlot
    {
        Head,
        Chest,
        Legs,
        Feet,
        Weapon,
        Shield,
        // Arrows
    };
}