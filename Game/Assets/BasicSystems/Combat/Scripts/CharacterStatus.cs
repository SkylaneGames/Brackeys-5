using System.Collections;
using System.Collections.Generic;
using Possession;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(CombatSystem))]
    public class CharacterStatus : MonoBehaviour
    {
        public HealthSystem HealthSystem { get; set; }
        public CombatSystem CombatSystem { get; set; }
        public DetectionSystem DetectionSystem { get; set; }
        public IPossessable Possessable { get; set; }

        public StatusBar HealthBar;
        public StatusBar RageBar;

        void Awake()
        {
            HealthSystem = GetComponent<HealthSystem>();
            CombatSystem = GetComponent<CombatSystem>();
            DetectionSystem = GetComponent<DetectionSystem>();
            Possessable = transform.parent.GetComponentInChildren<IPossessable>();

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
