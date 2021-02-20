using System.Collections;
using System.Collections.Generic;
using Combat;
using Possession;
using UnityEngine;

namespace NPC
{
    public class SpiritNPCController : NPCController
    {
        private SpiritController _spiritController;
        protected virtual PossessionSystem PossessionSystem => _spiritController.PossessionSystem;

        public override CharacterController Controller => PossessionSystem.IsPossessing ?
            PossessionSystem.PossessedCharacter.Controller
            : base.Controller;

        protected AbilitySystem AbilitySystem => _spiritController.AbilitySystem;

        protected override void Awake()
        {
            base.Awake();

            _spiritController = base.Controller as SpiritController;
        }

        protected override bool AttackPlayer(CharacterController player)
        {

            var availableAbilities = AbilitySystem.AvailableAbilities;

            foreach (var ability in availableAbilities)
            {
                var damageAbility = ability.Value as DamageAbility;
                if (damageAbility != null)
                {
                    if (damageAbility.Type == AbilityType.Ranged)
                    {
                        transform.LookAt(player.transform, Vector3.up);
                        AbilitySystem.Use(ability.Key);
                        return true;
                    }
                    else if (damageAbility.Type == AbilityType.Area)
                    {
                        var distance = (player.transform.position - transform.position).magnitude;
                        if (distance < damageAbility.Radius)
                        {
                            AbilitySystem.Use(ability.Key);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
