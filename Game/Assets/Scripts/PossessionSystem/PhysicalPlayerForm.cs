using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using Possession;
using UnityEngine;

[RequireComponent(typeof(Possess_CharacterMovement))]
public class PhysicalPlayerForm : MonoBehaviour
{
    public Possess_CharacterMovement MovementSystem { get; protected set; }
    public CharacterInteraction InteractionSystem { get; protected set; }

    private IPossessable Possessable;

    protected void OnPossessed()
    {
        InteractionSystem.UseHighlights = true;

        // TODO: Trigger getting up animation which will then enable the movement system.
        MovementSystem.enabled = true;
    }

    protected void OnPossessionReleased()
    {
        MovementSystem.StopMoving();
        MovementSystem.enabled = false;
        InteractionSystem.UseHighlights = false;

        // TODO: Trigger collapse to floor animation
    }

    void Awake()
    {
        MovementSystem = GetComponent<Possess_CharacterMovement>();
        InteractionSystem = GetComponentInChildren<CharacterInteraction>();
        Possessable = GetComponentInChildren<IPossessable>();

        if (Possessable != null)
        {
            Possessable.Possessed += OnPossessed;
            Possessable.PossessionReleased += OnPossessionReleased;
        }
    }
}
