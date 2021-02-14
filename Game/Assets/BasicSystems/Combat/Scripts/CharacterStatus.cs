using System.Collections;
using System.Collections.Generic;
using Possession;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(LifeSystem))]
    public class CharacterStatus : MonoBehaviour
    {
        public LifeSystem LifeSystem { get; set; }
        public DetectionSystem DetectionSystem { get; set; }
        public PossessionSystem PossessionSystem { get; set; }

        public GameObject StatusBarPrefab;

        [Range(0,10)]
        public int Willpower = 4;

        [Range(0,10)]
        public int Perception = 4;

        [Range(0,10)]
        public int Rage = 0;

        void Awake()
        {
            LifeSystem = GetComponent<LifeSystem>();
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
