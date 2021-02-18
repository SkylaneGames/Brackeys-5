using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "New Damage", menuName = "Abilities/Damage")]
    public class DamageAbility : Ability
    {
        public override void Use(AbilitySystem caller)
        {
            if (Type == AbilityType.Self)
            {
                return;
            }

            var spawnPosition = caller.transform.position;

            if (Type == AbilityType.Ranged)
            {
                spawnPosition += caller.transform.forward + Vector3.up * 2;
            }

            var magicObject = Instantiate(Prefab, spawnPosition, caller.Controller.transform.localRotation);

            if (Type == AbilityType.Area)
            {
                var magicScript = magicObject.GetComponent<AOEMagic>();
                magicScript.lifetime = CastTime;
                magicScript.Caller = caller.Controller.CombatSystem;
                
            }
            else if (Type == AbilityType.Ranged)
            {
                var magicScript = magicObject.GetComponent<RangedMagic>();
                magicScript.Caller = caller.Controller.CombatSystem;
            }
        }
    }
}
