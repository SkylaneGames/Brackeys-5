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

    public bool IsPossessed => PossessingCharacter != null;
    public PossessionSystem PossessingCharacter { get; set; } = null;

    public Transform Transform => transform;

    public string Name => name;

    public InteractionHighlight HighlightObject { get; set; }

    protected override void Awake()
    {
        base.Awake();
        HighlightObject = GetComponentInChildren<InteractionHighlight>();
    }

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
}
