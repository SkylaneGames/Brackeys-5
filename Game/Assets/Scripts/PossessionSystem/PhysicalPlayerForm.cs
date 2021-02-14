using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Possession
{
    [RequireComponent(typeof(Possess_CharacterMovement))]
    public class PhysicalPlayerForm : Possessable
    {
        public override Possess_CharacterMovement MovementSystem { get; protected set; }
        public override CharacterInteraction InteractionSystem { get; protected set; }

        protected override void OnPossessed()
        {
            InteractionSystem.UseHighlights = true;

            // TODO: Trigger getting up animation which will then enable the movement system.
            MovementSystem.enabled = true;
        }

        protected override void OnPossessionReleased()
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
        }
    }
}
