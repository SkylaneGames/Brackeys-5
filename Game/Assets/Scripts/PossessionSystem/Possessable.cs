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

        public bool IsPossessed { get; private set; }

        public Transform Transform => transform;

        public string Name => name;


        // public event Action Possessed;
        // public event Action PossessionReleased;


        public bool Possess()
        {
            if (IsPossessed)
            {
                return false;
            }

            IsPossessed = true;
            OnPossessed();

            return true;
        }

        public void ReleasePossession()
        {
            if (IsPossessed)
            {
                IsPossessed = false;
                OnPossessionReleased();
            }
        }

        public void Interact(GameObject interacter)
        {
            var interacterScript = interacter.GetComponent<PossessionSystem>();

            interacterScript.Possess(this);
        }

        public bool CanInteract(GameObject interacter)
        {
            return interacter.GetComponent<PossessionSystem>() != null;
        }

        public void Highlight()
        {
            Debug.Log($"Can interact with '{Name}'");
        }

        public void RemoveHighlight()
        {
            Debug.Log($"Can no longer interact with '{Name}'");
        }

        protected abstract void OnPossessed();
        protected abstract void OnPossessionReleased();
    }
}