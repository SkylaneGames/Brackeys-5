using System;
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
    public event Action<float> WillpowerChanged;

    public string Name => name;

    [SerializeField]
    [Range(0, 1)]
    private float willpower = 0.3f;
    public float Willpower => willpower;

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
}
