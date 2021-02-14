using System;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        public string Name;
        public Sprite Icon;

        public virtual void Use(GameObject parent = null)
        {
            Debug.Log("Using " + name);
        }
    }
}