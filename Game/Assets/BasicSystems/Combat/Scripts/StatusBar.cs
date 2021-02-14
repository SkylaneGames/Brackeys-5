using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    [RequireComponent(typeof(Slider))]
    public class StatusBar : MonoBehaviour
    {
        public Gradient gradient;
        public Image fill;

        [Tooltip("Number of seconds to display the health bar for after being hit. (Set 0 for it to always be visible)")]
        public float displayForSeconds = 3f;

        private Slider slider;

        private void UpdateValue(float value)
        {
            slider.value = value;
            fill.color = gradient.Evaluate(slider.normalizedValue);
            gameObject.SetActive(slider.normalizedValue < 1);
        }

        void Awake()
        {
            slider = GetComponent<Slider>();
        }
    }
}
