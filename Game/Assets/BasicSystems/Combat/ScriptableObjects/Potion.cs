using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
    public class Potion : Item
    {
        public PotionType PotionType;
        public int Strength;
        public IEnumerable<StatusEffect> Effects;
    }

    public struct StatusEffect
    {
        public EffectTypes EffectTypes;
        public int Strength;
    }

    public enum EffectTypes
    {
        IncreaseDamage
    }

    public enum PotionType
    {
        RestoreActionPoints
    }
}