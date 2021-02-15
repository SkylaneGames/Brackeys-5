using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using NPC;
using Possession;
using UnityEngine;

public class PossessableNPC : NPCController
{
    private IPossessable Possessable;

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

    protected override void Awake()
    {
        base.Awake();
        Possessable = GetComponentInChildren<IPossessable>();

        if (Possessable != null)
        {
            Possessable.Possessed += OnPossessed;
            Possessable.PossessionReleased += OnPossessionReleased;
        }
    }
}
