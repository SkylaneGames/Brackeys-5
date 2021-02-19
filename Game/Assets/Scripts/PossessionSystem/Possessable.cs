using System;
using Combat;
using Interaction;
using UnityEngine;

namespace Possession
{
    [RequireComponent(typeof(Collider))]
    public class Possessable : MonoBehaviour, IPossessable, IInteractable
    {
        public event Action Possessed;
        public event Action PossessionReleased;

        public bool IsPossessed => PossessingCharacter != null;

        public PossessionSystem PossessingCharacter { get; set; } = null;

        public string Name => name;

        public InteractionHighlight HighlightObject { get; protected set; }

        public float Rage
        {
            get { return CombatSystem.Rage; }
            set { CombatSystem.Rage = value; }
        }

        [SerializeField]
        [Range(0, 1)]
        private float BaseResistance = 0.2f;

        [SerializeField]
        [Range(0, 1)]
        private float RagePerPossession = 0.1f;

        public float Resistance => BaseResistance + Rage;

        public CharacterController Controller { get; private set; }

        public Transform Transform => Controller.transform;

        private CombatSystem CombatSystem;

        void Awake()
        {
            Controller = GetComponentInParent<CharacterController>();
            HighlightObject = GetComponentInChildren<InteractionHighlight>();
            CombatSystem = transform.parent.GetComponentInChildren<CombatSystem>();
        }

        public bool Possess(PossessionSystem possessingCharacter)
        {
            if (IsPossessed)
            {
                return false;
            }

            PossessingCharacter = possessingCharacter;
            Possessed?.Invoke();

            return true;
        }

        public void ReleasePossession(bool isOwnPhysicalForm)
        {
            if (IsPossessed)
            {
                PossessingCharacter = null;
                if (!isOwnPhysicalForm)
                {
                    Rage += RagePerPossession;
                }
                PossessionReleased?.Invoke();
            }
        }

        public void Interact(CharacterController interacter, Action callback)
        {
            var interactersPossessionSystem = GetPossessionSystem(interacter);

            if (interactersPossessionSystem == null)
            {
                Debug.LogWarning($"No possession system found on {interacter.name}");
                callback?.Invoke();
                return;
            }

            interactersPossessionSystem.Possess(this, () => { HighlightObject.Hide(); callback?.Invoke(); });
        }

        public bool CanInteract(CharacterController interacter)
        {
            if (IsPossessed || Controller.CombatSystem.HealthSystem.IsDead)
            {
                return false;
            }

            var interactersPossessionSystem = GetPossessionSystem(interacter);

            if (interactersPossessionSystem == null)
            {
                return false;
            }

            // You can always possess your own form
            if (interactersPossessionSystem.PhysicalForm == this)
            {
                return true;
            }

            // Characters can only possess characters whose rage level is lower than possession power
            return interactersPossessionSystem.PoessessionPower > Resistance;
        }

        private PossessionSystem GetPossessionSystem(CharacterController interacter)
        {
            var interactersPossessionSystem = interacter.GetComponent<PossessionSystem>();
            if (interactersPossessionSystem == null)
            {
                interactersPossessionSystem = interacter.GetComponentInChildren<IPossessable>()?.PossessingCharacter;
            }

            return interactersPossessionSystem;
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
}