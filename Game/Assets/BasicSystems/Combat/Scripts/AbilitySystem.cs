using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Combat
{
    public class AbilitySystem : MonoBehaviour
    {
        public List<Ability> Abilities = new List<Ability>(5);
        public float[] cooldowns { get; private set; }

        public IDictionary<int, Ability> AvailableAbilities => Abilities.Where(p => p != null && cooldowns[Abilities.IndexOf(p)] <= 0).ToDictionary(p => Abilities.IndexOf(p), p => p);

        public SpiritController Controller { get; private set; }

        public bool IsCasting { get; private set; } = false;
        public AudioClip[] audioClips;

        void Awake()
        {
            Controller = GetComponentInParent<SpiritController>();
        }

        void Start()
        {
            cooldowns = new float[Abilities.Count];
        }

        void Update()
        {
            UpdateCooldowns();
        }

        private void UpdateCooldowns()
        {
            for (int i = 0; i < Abilities.Count; i++)
            {
                if (cooldowns[i] > 0)
                {
                    cooldowns[i] -= Time.deltaTime;
                    cooldowns[i] = cooldowns[i] < 0 ? 0 : cooldowns[i];
                }
            }
        }

        public void Use(int ability)
        {
            if (Controller.PossessionSystem.IsPossessing)
            {
                return;
            }

            if (IsCasting)
            {
                return;
            }

            if (ability < 0 || ability >= Abilities.Count || Abilities[ability] == null)
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
            GetComponentInParent<AudioSource>().PlayOneShot(audioClips[ability],0.05f);
        }

        private IEnumerator TriggerAbilityFinished(int ability)
        {
            yield return new WaitForSeconds(Abilities[ability].CastTime);
            cooldowns[ability] = Abilities[ability].Cooldown;
            IsCasting = false;
        }
    }
}