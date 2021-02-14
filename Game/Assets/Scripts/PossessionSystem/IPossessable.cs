using System;
using UnityEngine;

namespace Possession
{
    public interface IPossessable
    {
        // event Action Possessed;
        // event Action PossessionReleased;
        event Action<float> WillpowerChanged;
        Transform Transform { get; }
        float Willpower { get; }
        Possess_CharacterMovement MovementSystem { get; }
        CharacterInteraction InteractionSystem { get; }
        PossessionSystem PossessingCharacter { get; }
        bool IsPossessed { get; }

        bool Possess(PossessionSystem possessingCharacter);
        void ReleasePossession(float dWillpower = 0);
    }
}