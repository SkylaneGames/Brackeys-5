using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class AbilitySystem : MonoBehaviour
    {
        public List<Ability> Abilities;

        public SpiritController Controller { get; private set; }

        void Awake()
        {
            Controller = GetComponentInParent<SpiritController>();
        }

        public void Use(int ability)
        {
            if (ability < 0 || ability >= Abilities.Count)
            {
                Debug.Log($"Invalid ability '{ability}'");
                return;
            }

            Abilities[ability].Use(this);
        }
    }
}