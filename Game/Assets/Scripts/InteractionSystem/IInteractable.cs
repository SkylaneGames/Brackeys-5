using UnityEngine;

namespace Interaction
{
    public interface IInteractable
    {
        string Name { get; }
        Transform Transform { get; }
        InteractionHighlight HighlightObject { get; }
        void Interact(GameObject interacter);
        bool CanInteract(GameObject interacter);
        void Highlight();
        void RemoveHighlight();
    }
}