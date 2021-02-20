using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Interaction;
using UnityEngine;


namespace NPC
{
    public delegate void PerceptionChanged(float perception);

    [RequireComponent(typeof(SphereCollider))]
    public class CharacterVision : MonoBehaviour
    {
        public event PerceptionChanged PerceptionChanged;

        [SerializeField]
        [Range(0, 1)]
        private float perception = 0.4f;
        public float Perception
        {
            get { return perception; }
            set
            {
                perception = Mathf.Clamp01(value);
                OnPerceptionChanged(perception);
            }
        }

        public float threshold = 0.5f;

        [SerializeField]
        [Range(0, 360)]
        private float FieldOfView = 90f;

        [SerializeField]
        [Range(0, 50)]
        public float MaximumViewDistance = 30f;

        public float ViewDistance => MaximumViewDistance * Perception;

        public float LocalAwareness = 1f;

        private SphereCollider visionSphere;

        protected ISet<IInteractable> _interactables = new HashSet<IInteractable>();
        public IEnumerable<IInteractable> Interactables => _interactables.Where(p => CanSeeInteractable(p));

        protected ISet<CharacterController> _characters = new HashSet<CharacterController>();
        public IEnumerable<CharacterController> Characters => _characters.Where(p => CanSeeCharacter(p));

        public NPCController NPCController { get; private set; }

        void Awake()
        {
            visionSphere = GetComponent<SphereCollider>();
            NPCController = GetComponentInParent<NPCController>();
        }

        private void OnPerceptionChanged(float perception)
        {
            visionSphere.radius = ViewDistance;

            PerceptionChanged?.Invoke(perception);
        }

        // Start is called before the first frame update
        void Start()
        {
            OnPerceptionChanged(Perception);
        }

        void Update()
        {

        }

        protected void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Interaction"))
            {
                var interactable = collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    _interactables.Add(interactable);
                }
            }
            else
            {
                var character = collider.GetComponentInParent<CharacterController>();

                if (character != null)
                {
                    _characters.Add(character);
                }
            }
        }

        protected void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Interaction"))
            {
                var interactable = collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    _interactables.Remove(interactable);
                }
            }
            else
            {
                var character = collider.GetComponentInParent<CharacterController>();

                if (character != null)
                {
                    _characters.Remove(character);
                }
            }
        }

        protected bool CanSeeInteractable(IInteractable interactable)
        {
            if (!interactable.CanInteract(NPCController.Controller))
            {
                return false;
            }
            else
            {
                return IsWithinView(interactable.Transform);
            }
        }

        protected bool CanSeeCharacter(CharacterController character)
        {
            if (character.CombatSystem.HealthSystem.IsDead)
            {
                return false;
            }
            
            // Only spirits can see other spirits
            if (character.CharacterType == CharacterType.Spirit && NPCController.Controller.CharacterType != CharacterType.Spirit)
            {
                return false;
            }
            // Spirits don't interact with physical players (possessable is handled in the Interactables not characters)
            else if (character.CharacterType == CharacterType.Physical && NPCController.Controller.CharacterType == CharacterType.Spirit)
            {
                return false;
            }
            else if (character.CharacterType == CharacterType.Spirit)
            {
                var spirit = character as SpiritController;
                // Spirits currently possessing are hidden to other spirits
                if (spirit.PossessionSystem.IsPossessing)
                {
                    return false;
                }
            }
            
            return IsWithinView(character.transform);
        }

        protected bool IsWithinView(Transform target)
        {
            var direction = target.position - transform.position;
            var distance = direction.magnitude;

            if (distance < LocalAwareness)
            {
                return true;
            }

            direction = direction.normalized;
            var angle = Vector3.Angle(transform.forward, direction);

            if (Mathf.Abs(angle) <= FieldOfView / 2.0f)
            {
                return IsUnobscured(direction, distance, target);
            }

            return false;
        }

        protected bool IsUnobscured(Vector3 direction, float distance, Transform target)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, ViewDistance, LayerMask.GetMask("Default", "Physical", "Ignore Raycast")))
            {
                return Mathf.Abs(hit.distance - distance) <= threshold;
            }


            return true;
        }

        void OnDrawGizmosSelected()
        {
            float totalFOV = FieldOfView;
            float rayRange = ViewDistance;
            float halfFOV = totalFOV / 2.0f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
            Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
            Gizmos.color = Color.magenta;

            for (float i = -0.4f; i <= 0.4f; i += 0.1f)
            {
                Quaternion rayRotation = Quaternion.AngleAxis(FieldOfView * i, Vector3.up);
                Vector3 rayDirection = rayRotation * transform.forward;
                Gizmos.DrawRay(transform.position, rayDirection * rayRange);

            }

            foreach (var item in Characters)
            {
                Gizmos.DrawIcon(item.transform.position, "animationvisibilitytoggleon@2x", true);
            }

            foreach (var item in Interactables)
            {
                Gizmos.DrawIcon(item.Transform.position, "animationvisibilitytoggleon@2x", true);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, LocalAwareness);
        }

    }
}
