using System.Collections;
using System.Collections.Generic;
using Possession;
using UnityEngine;

namespace Combat
{
    public class CharacterStatus : MonoBehaviour
    {
        public HealthSystem HealthSystem;
        public CombatSystem CombatSystem;
        public DetectionSystem DetectionSystem;
        public IPossessable Possessable;

        public StatusBar HealthBar;
        public StatusBar RageBar;

        void Awake()
        {
            HealthSystem = HealthSystem ?? transform.parent.GetComponentInChildren<HealthSystem>();
            CombatSystem = CombatSystem ?? transform.parent.GetComponentInChildren<CombatSystem>();
            DetectionSystem = DetectionSystem ?? transform.parent.GetComponentInChildren<DetectionSystem>();
            Possessable = Possessable ?? transform.parent.GetComponentInChildren<IPossessable>();

            HealthSystem.HealthChanged += (health) => HealthBar.UpdateValue(health);
            CombatSystem.RageChanged += (rage) => RageBar.UpdateValue(rage);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
