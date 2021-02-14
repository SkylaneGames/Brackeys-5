using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        string Name { get; }
        Transform Transform { get; }
        void Interact(GameObject interacter);
        bool CanInteract(GameObject interacter);
        void Highlight();
        void RemoveHighlight();
    }
}