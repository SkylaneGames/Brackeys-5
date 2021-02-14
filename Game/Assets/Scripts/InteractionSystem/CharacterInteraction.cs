using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CharacterInteraction : MonoBehaviour
{
    // Use this to disable highlights when NPCs interact with objects.
    public bool UseHighlights = true;

    public IInteractable LastInteractable = null;

    private void OnTriggerStay(Collider collider)
    {
        var interactable = collider.GetComponent<IInteractable>();

        if (interactable != null && interactable != LastInteractable && interactable.CanInteract(transform.parent.gameObject))
        {
            if (LastInteractable != null)
            {
                // Get the closest interactable and if it's the new one, replace the current interactable object with it.
                var currentDistance = (transform.position - LastInteractable.Transform.position).sqrMagnitude;
                var newDistance = (transform.position - interactable.Transform.position).sqrMagnitude;

                if (newDistance < currentDistance)
                {
                    LastInteractable.RemoveHighlight();
                    LastInteractable = interactable;

                    if (UseHighlights)
                    {
                        interactable.Highlight();
                    }
                }
            }
            else
            {
                if (UseHighlights)
                {
                    interactable.Highlight();
                }
                
                LastInteractable = interactable;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        var interactable = collider.GetComponent<IInteractable>();

        if (interactable != null)
        {
            if (LastInteractable == interactable)
            {
                if (UseHighlights)
                {
                    interactable.RemoveHighlight();
                }
                LastInteractable = null;
            }
        }
    }
}
