
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "New Heal", menuName = "Abilities/Heal")]
    public class HealAbility : Ability
    {
        public int Health = 10;

        public override void Use(AbilitySystem caller)
        {
            switch (Type)
            {
                case AbilityType.Self:
                    return;
                case AbilityType.Area:
                case AbilityType.Ranged:
                    return;
            }

            var spawnPosition = caller.transform.position + Vector3.up;
            
            if (Type == AbilityType.Ranged)
            {
                spawnPosition += caller.transform.forward;
            }
            else if (Type == AbilityType.Self)
            {
                caller.Controller.CombatSystem.HealthSystem.Heal(Health);
            }

            var magicObject = Instantiate(Prefab, spawnPosition, caller.Controller.transform.rotation);
        }
    }
}