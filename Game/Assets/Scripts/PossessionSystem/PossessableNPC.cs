using System.Collections;
using System.Collections.Generic;
using Interaction;
using NPC;
using Possession;
using UnityEngine;

public class PossessableNPC : NPCController, IPossessable, IInteractable
{

    protected void OnPossessed()
    {
        this.enabled = false;
        InteractionSystem.UseHighlights = true;
    }

    protected void OnPossessionReleased()
    {
        MovementSystem.StopMoving();
        InteractionSystem.UseHighlights = false;
        this.enabled = true;
    }

    public bool IsPossessed { get; private set; }

    public Transform Transform => transform;

    public string Name => name;


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
}
