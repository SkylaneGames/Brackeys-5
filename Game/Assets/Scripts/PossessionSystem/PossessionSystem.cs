using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Possession
{
    [RequireComponent(typeof(Collider))]
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

        void Awake()
        {
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

        public void Possess(IPossessable character)
        {
            if (character.Possess(this))
            {
                if (PossessedCharacter != null)
                {
                    ReleaseCurrentPossession(true);
                }

                PossessedCharacter = character;
                CameraSystem.Target = PossessedCharacter.Transform;

                // Handle Spirit form

                HideSpiritForm();
                CharacterPossessed?.Invoke();
            }
        }

        private void ShowSpiritForm()
        {
            // TODO: Change to use an animation (which will set this at the end of the animation)
            SpiritForm.SetActive(true);
        }

        private void HideSpiritForm()
        {
            // TODO: Change to use an animation (which will set this at the end of the animation)
            SpiritForm.SetActive(false);
            
        }

        public void ReleaseCurrentPossession(bool repossession = false)
        {
            if (PossessedCharacter == null)
            {
                return;
            }

            PossessedCharacter.ReleasePossession();

            // Handle spirit form
            if (!repossession)
            {
                transform.position = GetPositionAfterPossession();
                ShowSpiritForm();
                CameraSystem.Target = transform;
            }
            else
            {
                // TODO: Play an animation of the spirit jumping between bodies
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
