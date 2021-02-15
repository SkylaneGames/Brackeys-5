using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Possession
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Possess_CharacterMovement))]
    public class PossessionSystem : MonoBehaviour
    {
        public bool IsPossessing => PossessedCharacter != null;

        public IPossessable PossessedCharacter = null;

        public Possessable PhysicalForm = null;

        public GameObject SpiritForm = null;

        private Possess_CameraFollow CameraSystem;

        [SerializeField]
        [Range(0,1)]
        private float possessionPower = 0.4f;

        public float PoessessionPower => possessionPower;

        public event Action CharacterPossessed;
        public event Action PossessionReleased;

        protected Animator _animator;
        protected Possess_CharacterMovement _movement;

        private bool animComplete = true;

        private Action currentInteractionCallback = null;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _movement = GetComponent<Possess_CharacterMovement>();

            CameraSystem = FindObjectOfType<Possess_CameraFollow>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (PossessedCharacter != null)
            {
                HideSpiritForm();
                CameraSystem.Target = PossessedCharacter.Transform;
            }
        }

        void FixedUpdate()
        {
            if (PossessedCharacter != null && animComplete)
            {
                transform.position = PossessedCharacter.Transform.position;
                transform.rotation = PossessedCharacter.Transform.rotation;
            }
        }

        public void Possess(IPossessable character, Action callback)
        {
            if (character.Possess(this))
            {
                currentInteractionCallback = callback;
                bool isRepossession = PossessedCharacter != null;
                _movement.StopMoving();
                var lookTarget = character.Transform.position;
                lookTarget.y = transform.position.y;
                transform.LookAt(lookTarget, Vector3.up);
                if (isRepossession)
                {
                    ReleaseCurrentPossession(true);
                    animComplete = false;
                }

                PossessedCharacter = character;
                CameraSystem.Target = PossessedCharacter.Transform;

                // Handle Spirit form

                if (!isRepossession)
                {
                    HideSpiritForm();
                }
            }
        }

        private void ShowSpiritForm()
        {
            // TODO: Change to use an animation (which will set this at the end of the animation)
            animComplete = false;
            _animator.SetTrigger("Unpossess");
            // SpiritForm.SetActive(true);
        }

        private void HideSpiritForm()
        {
            // TODO: Change to use an animation (which will set this at the end of the animation)
            animComplete = false;
            _animator.SetTrigger("Possess");
            // SpiritForm.SetActive(false);
            
        }

        public void PossessionComplete()
        {
            animComplete = true;
            CharacterPossessed?.Invoke();
            currentInteractionCallback?.Invoke();
            currentInteractionCallback = null;
        }

        public void RepossessionComplete()
        {
            animComplete = true;
            CharacterPossessed?.Invoke();
            currentInteractionCallback?.Invoke();
            currentInteractionCallback = null;
        }

        public void UnpossessionComplete()
        {
            animComplete = true;
            PossessionReleased?.Invoke();
            currentInteractionCallback?.Invoke();
            currentInteractionCallback = null;
        }

        public void ReleaseCurrentPossession(bool repossession = false, Action callback = null)
        {
            if (PossessedCharacter == null)
            {
                return;
            }

            PossessedCharacter.ReleasePossession();

            // Handle spirit form
            if (!repossession)
            {
                currentInteractionCallback = callback;
                transform.position = GetPositionAfterPossession();
                ShowSpiritForm();
                CameraSystem.Target = transform;
            }
            else
            {
                _animator.SetTrigger("Repossess");
            }

            PossessedCharacter = null;
            PossessionReleased?.Invoke();
        }

        private Vector3 GetPositionAfterPossession()
        {
            return PossessedCharacter.Transform.position - PossessedCharacter.Transform.forward;
        }
    }
}
