using System;
using UnityEngine;

namespace Possession
{
    public interface IPossessable
    {
        event Action Possessed;
        event Action PossessionReleased;

        CharacterController Controller { get; }
        float Rage { get; }

        PossessionSystem PossessingCharacter { get; }
        bool IsPossessed { get; }

        bool Possess(PossessionSystem possessingCharacter);
        void ReleasePossession(bool isOwnPhysicalForm);
    }
}