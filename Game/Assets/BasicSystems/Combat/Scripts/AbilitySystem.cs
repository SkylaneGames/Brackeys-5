using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class AbilitySystem : MonoBehaviour
    {
        public Ability[] Abilities = new Ability[5];
        private float[] cooldowns;

        public SpiritController Controller { get; private set; }

        public bool IsCasting { get; private set; } = false;

        void Awake()
        {
            Controller = GetComponentInParent<SpiritController>();
        }

        void Start()
        {
            cooldowns = new float[Abilities.Length];
        }

        void Update()
        {
            UpdateCooldowns();
        }

        private void UpdateCooldowns()
        {
            for (int i = 0; i < Abilities.Length; i++)
            {
                if (cooldowns[i] > 0)
                {
                    cooldowns[i] -= Time.deltaTime;
                    cooldowns[i] = cooldowns[i] < 0 ? 0: cooldowns[i];
                }
            }
        }

        public void Use(int ability)
        {
            if (IsCasting)
            {
                return;
            }

            if (ability < 0 || ability >= Abilities.Length || Abilities[ability] == null)
            {
                Debug.LogWarning($"Invalid ability '{ability}'");
                return;
            }

            if (cooldowns[ability] > 0)
            {
                Debug.Log("Ability on cooldowns");
                return;
            }

            Debug.Log($"Using ability {ability}");

            IsCasting = true;

            StartCoroutine("TriggerAbilityFinished", ability);
            Abilities[ability].Use(this);
        }

        private IEnumerator TriggerAbilityFinished(int ability)
        {
            yield return new WaitForSeconds(Abilities[ability].CastTime);
            cooldowns[ability] = Abilities[ability].Cooldown;
            IsCasting = false;
        }
    }
}