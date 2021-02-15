using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class LifeSystemUI : MonoBehaviour
    {
        public GameObject HealthBarPrefab;

        private IEnumerable<StatusBar> healthBars;

        void Start()
        {
            var lifeSystems = FindObjectsOfType<LifeSystem>();
            var healthBars = new List<StatusBar>();

            foreach (var lifeSystem in lifeSystems)
            {
                var newObject = Instantiate(HealthBarPrefab, lifeSystem.transform);
                newObject.GetComponent<RectTransform>().localPosition = Vector3.up * 4;
                var newHealthBar = newObject.GetComponent<StatusBar>();

                healthBars.Add(newHealthBar);
            }

            this.healthBars = healthBars;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
