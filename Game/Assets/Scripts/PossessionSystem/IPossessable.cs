using System;
using UnityEngine;

namespace Possession
{
    public interface IPossessable
    {
        // event Action Possessed;
        // event Action PossessionReleased;
        Transform Transform { get; }
        Possess_CharacterMovement MovementSystem { get; }
        CharacterInteraction InteractionSystem { get; }
        bool IsPossessed { get; }

        bool Possess();
        void ReleasePossession();
    }
}