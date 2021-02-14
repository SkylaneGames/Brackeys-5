using UnityEngine;

namespace NPC
{
    [RequireComponent(typeof(Possess_CharacterMovement))]
    public class NPCController : MonoBehaviour
    {
        public Possess_CharacterMovement MovementSystem { get; protected set; }
        public CharacterInteraction InteractionSystem { get; protected set; }

        protected virtual void Awake()
        {
            MovementSystem = GetComponent<Possess_CharacterMovement>();
            InteractionSystem = GetComponentInChildren<CharacterInteraction>();
        }
    }
}