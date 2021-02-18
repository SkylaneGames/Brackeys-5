using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public enum AbilityType
    {
        Self, Area, Ranged
    }

    public abstract class Ability : ScriptableObject
    {
        public string Name;
        public Sprite Icon;

        public AbilityType Type;
        public GameObject Prefab;

        public virtual void Use(AbilitySystem caller)
        {
            var used = OnUse(caller);

            if (used)
            {
                Instantiate(Prefab, caller.transform);
            }
        }

        protected abstract bool OnUse(AbilitySystem caller);
    }

    [CreateAssetMenu(fileName = "New Heal", menuName = "Abilities/Heal")]
    public class HealAbility : Ability
    {
        public int Health = 10;

        protected override bool OnUse(AbilitySystem caller)
        {
            switch (Type)
            {
                case AbilityType.Self:
                    caller.Controller.CombatSystem.HealthSystem.Heal(Health);
                    return true;
                case AbilityType.Area:
                case AbilityType.Ranged:
                    // Do nothing (the prefab object will have a collider which triggers any effects)
                    // just indicate the ability was used so the prefab is instantiated in the Ability class.
                    return true;
            }

            return false;
        }
    }

    [CreateAssetMenu(fileName = "New Damage", menuName = "Abilities/Damage")]
    public class DamageAbility : Ability
    {
        public List<DamageInfo> Damage;

        protected override bool OnUse(AbilitySystem caller)
        {
            if (Type == AbilityType.Self)
            {
                return false;
            }

            return true;
        }
    }

    [CreateAssetMenu(fileName = "New Force", menuName = "Abilities/Force")]
    public class ForceAbility : Ability
    {
        public float Power;

        protected override bool OnUse(AbilitySystem caller)
        {
            if (Type == AbilityType.Self)
            {
                return false;
            }

            return true;
        }
    }
}
