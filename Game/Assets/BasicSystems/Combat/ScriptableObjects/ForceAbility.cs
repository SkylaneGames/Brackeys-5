using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "New Force", menuName = "Abilities/Force")]
    public class ForceAbility : Ability
    {
        public float Power = 1f;

        public override void Use(AbilitySystem caller)
        {
            if (Type == AbilityType.Self)
            {
                return;
            }

        }
    }
}
