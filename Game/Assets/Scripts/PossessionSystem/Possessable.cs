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

        public void ReleasePossession()
        {
            if (IsPossessed)
            {
                PossessingCharacter = null;
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
            return interacter.GetComponent<PossessionSystem>() != null ? true :
                interacter.GetComponent<IPossessable>()?.PossessingCharacter != null;
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