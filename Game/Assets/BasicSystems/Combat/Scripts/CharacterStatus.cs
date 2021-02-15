using System.Collections;
using System.Collections.Generic;
using Possession;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(LifeSystem))]
    [RequireComponent(typeof(CombatSystem))]
    public class CharacterStatus : MonoBehaviour
    {
        public LifeSystem LifeSystem { get; set; }
        public CombatSystem CombatSystem { get; set; }
        public DetectionSystem DetectionSystem { get; set; }
        public IPossessable Possessable { get; set; }

        private StatusBar HealthBar;
        private StatusBar RageBar;

        public GameObject StatusBarPrefab;

        void Awake()
        {
            LifeSystem = GetComponent<LifeSystem>();
            CombatSystem = GetComponent<CombatSystem>();
            DetectionSystem = GetComponent<DetectionSystem>();
            Possessable = transform.parent.GetComponentInChildren<IPossessable>();
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
