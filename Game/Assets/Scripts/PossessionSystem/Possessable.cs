using System;
using Interaction;
using UnityEngine;

namespace Possession
{
    [RequireComponent(typeof(Possess_CharacterMovement))]
    [RequireComponent(typeof(Collider))]
    public abstract class Possessable : MonoBehaviour, IPossessable, IInteractable
    {
        public abstract Possess_CharacterMovement MovementSystem { get; protected set; }
        public abstract CharacterInteraction InteractionSystem { get; protected set; }

        public bool IsPossessed => PossessingCharacter != null;

        public PossessionSystem PossessingCharacter { get; set; } = null;

        public Transform Transform => transform;

        public string Name => name;

        public abstract InteractionHighlight HighlightObject { get; protected set; }

        [SerializeField]
        [Range(0, 1)]
        private float willpower = 0.3f;

        public event Action<float> WillpowerChanged;

        public float Willpower => willpower;

        public bool Possess(PossessionSystem possessingCharacter)
        {
            if (IsPossessed)
            {
                return false;
            }

            PossessingCharacter = possessingCharacter;
            OnPossessed();

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
                OnPossessionReleased();
            }
        }

        public void Interact(GameObject interacter)
        {
            var interactersPossessionSystem = interacter.GetComponent<PossessionSystem>();
            if (interactersPossessionSystem == null)
            {
                interactersPossessionSystem = interacter.GetComponent<IPossessable>()?.PossessingCharacter;
            }

            HighlightObject.Hide();
            interactersPossessionSystem.Possess(this);
        }

        public bool CanInteract(GameObject interacter)
        {
            var interactersPossessionSystem = interacter.GetComponent<PossessionSystem>();
            if (interactersPossessionSystem == null)
            {
                interactersPossessionSystem = interacter.GetComponent<IPossessable>()?.PossessingCharacter;
            }

            if (interactersPossessionSystem == null)
            {
                return false;
            }

            // Characters can only possess characters whose willpower is lower than possession power.
            return interactersPossessionSystem.PoessessionPower > Willpower;
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

        protected abstract void OnPossessed();
        protected abstract void OnPossessionReleased();
    }
}