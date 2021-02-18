using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public enum AbilityType
    {
        Self, Area, Ranged
    }

    public abstract class Ability : ScriptableObject
    {
        public string Name;
        public Sprite Icon;

        public float CastTime = 1f;
        public float Cooldown = 0f;

        public AbilityType Type;
        public GameObject Prefab;

        public abstract void Use(AbilitySystem caller);
    }
}
