using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public class InteractionHighlight : MonoBehaviour
    {
        private MeshRenderer indicator;

        void Awake()
        {
            indicator = GetComponent<MeshRenderer>();
        }

        void Start()
        {
            Hide();
        }

        public void Show()
        {
            indicator.enabled = true;
        }

        public void Hide()
        {
            indicator.enabled = false;
        }
    }
}
