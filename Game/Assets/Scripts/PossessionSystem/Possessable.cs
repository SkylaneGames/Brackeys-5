using System;
using Interaction;
using UnityEngine;

namespace Possession
{
    [RequireComponent(typeof(Collider))]
    public class Possessable : MonoBehaviour, IPossessable, IInteractable
    {
        public event Action Possessed;
        public event Action PossessionReleased;

        public bool IsPossessed => PossessingCharacter != null;

        public PossessionSystem PossessingCharacter { get; set; } = null;

        public Transform Transform => transform.parent;

        public string Name => name;

        public InteractionHighlight HighlightObject { get; protected set; }

        [SerializeField]
        [Range(0, 1)]
        private float willpower = 0.3f;

        public event Action<float> WillpowerChanged;

        public float Willpower => willpower;

        void Awake()
        {
            HighlightObject = GetComponentInChildren<InteractionHighlight>();
        }

        public bool Possess(PossessionSystem possessingCharacter)
        {
            if (IsPossessed)
            {
                return false;
            }

            PossessingCharacter = possessingCharacter;
            Possessed?.Invoke();

            return true;
        }

        public void ReleasePossession(float dWillpower = 0)
        {
            if (IsPossessed)
            {
                PossessingCharacter = null;
                willpower += dWillpower;
                willpower = Mathf.Clamp01(willpower);
                WillpowerChanged?.Invoke(Willpower);
                PossessionReleased?.Invoke();
            }
        }

        public void Interact(GameObject interacter, Action callback)
        {
            var interactersPossessionSystem = GetPossessionSystem(interacter);

            if (interactersPossessionSystem == null)
            {
                Debug.LogWarning($"No possession system found on {interacter.name}");
                callback?.Invoke();
                return;
            }

            HighlightObject.Hide();
            interactersPossessionSystem.Possess(this, callback);
        }

        public bool CanInteract(GameObject interacter)
        {
            var interactersPossessionSystem = GetPossessionSystem(interacter);

            if (interactersPossessionSystem == null)
            {
                return false;
            }

            // Characters can only possess characters whose willpower is lower than possession power.
            return interactersPossessionSystem.PoessessionPower > Willpower;
        }

        private PossessionSystem GetPossessionSystem(GameObject interacter)
        {
            var interactersPossessionSystem = interacter.GetComponent<PossessionSystem>();
            if (interactersPossessionSystem == null)
            {
                interactersPossessionSystem = interacter.GetComponentInChildren<IPossessable>()?.PossessingCharacter;
            }

            return interactersPossessionSystem;
        }

        public void Highlight()
        {
            // Debug.Log($"Can interact with '{Name}'");
            HighlightObject?.Show();
        }

        public void RemoveHighlight()
        {
            // Debug.Log($"Can no longer interact with '{Name}'");
            HighlightObject?.Hide();
        }
    }
}