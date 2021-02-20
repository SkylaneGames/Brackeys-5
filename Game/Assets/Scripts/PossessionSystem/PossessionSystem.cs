using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Possession
{
    [RequireComponent(typeof(Animator))]
    public class PossessionSystem : MonoBehaviour
    {
        private class PossessionArgs
        {
            public IPossessable character;
            public Action callback;
        }

        public bool IsPossessing => PossessedCharacter != null;

        [SerializeField]
        private Possessable InitPossessedCharacter = null;
        public IPossessable PossessedCharacter = null;

        public Possessable PhysicalForm = null;

        //private Possess_CameraFollow CameraSystem;

        [SerializeField]
        [Range(0, 1)]
        private float possessionPower = 0.4f;

        public float PoessessionPower => possessionPower;

        public event Action CharacterPossessed;
        public event Action PossessionReleased;

        protected Animator _animator;

        public CharacterController Controller { get; set; }

        private bool animComplete = true;

        private PossessionArgs nextPossession = null;

        private Action unpossessionCallback = null;
        public GameObject SpiritParticles;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            Controller = GetComponent<CharacterController>();

            //CameraSystem = FindObjectOfType<Possess_CameraFollow>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (InitPossessedCharacter != null)
            {
                nextPossession = new PossessionArgs
                {
                    character = InitPossessedCharacter
                };
                
                PossessionComplete(true);
                PossessedCharacter.Possess(this);
                //CameraSystem.Target = PossessedCharacter.Controller.transform;
            }
        }

        void FixedUpdate()
        {
            if (PossessedCharacter != null && animComplete)
            {
                transform.position = PossessedCharacter.Controller.transform.position;
                transform.rotation = PossessedCharacter.Controller.transform.rotation;
            }
        }

        public void Possess(IPossessable character, Action callback)
        {
            if (character.Possess(this))
            {
                nextPossession = new PossessionArgs
                {
                    callback = callback,
                    character = character
                };

                bool isRepossession = PossessedCharacter != null;

                Controller.MovementSystem.StopMoving();

                var lookTarget = character.Controller.transform.position;
                lookTarget.y = transform.position.y;
                transform.LookAt(lookTarget, Vector3.up);

                if (isRepossession)
                {
                    ReleaseCurrentPossession(true);
                }

                // Handle Spirit form

                if (!isRepossession)
                {
                    HideSpiritForm();
                }
            }
        }

        private void ShowSpiritForm()
        {
            animComplete = false;
            SpiritParticles.SetActive(true);
            SpiritParticles.GetComponent<Animator>().SetTrigger("Unpossess");
            SpiritParticles.GetComponent<AudioSource>().PlayOneShot(SpiritParticles.GetComponent<AudioSource>().clip);
            StartCoroutine("CallAfterUnpossession");
            //_animator.SetTrigger("Unpossess");
        }

        private void HideSpiritForm()
        {
            animComplete = false;
            SpiritParticles.SetActive(true);
            SpiritParticles.GetComponent<Animator>().SetTrigger("Possess");
            SpiritParticles.GetComponent<AudioSource>().PlayOneShot(SpiritParticles.GetComponent<AudioSource>().clip);
            StartCoroutine("CallAfterPossession");
            //_animator.SetTrigger("Possess");

        }

        private IEnumerator CallAfterPossession()
        {
            yield return new WaitForSeconds(3);
            SpiritParticles.SetActive(false);
            PossessionComplete();
        }

        private IEnumerator CallAfterRepossession()
        {
            yield return new WaitForSeconds(3);
            RepossessionComplete();
        }

        private IEnumerator CallAfterUnpossession()
        {
            yield return new WaitForSeconds(2.9f);
            SpiritParticles.SetActive(false);
            UnpossessionComplete();
        }

        private void OnPossessionComplete(bool init = false)
        {
            CharacterPossessed?.Invoke();

            PossessedCharacter = nextPossession.character;
            //CameraSystem.Target = PossessedCharacter.Controller.transform;

            if (PossessedCharacter.RequiresSpirit && !init)
            {
                PossessedCharacter.Controller.Animator.SetTrigger("Possessed");
            }

            nextPossession.callback?.Invoke();
            nextPossession = null;
        }

        public void PossessionComplete(bool init = false)
        {
            Debug.Log("Possession complete");
            animComplete = true;
            OnPossessionComplete(init);
        }

        public void RepossessionComplete()
        {
            Debug.Log("repossession complete");
            animComplete = true;

            OnPossessionComplete();
        }

        public void UnpossessionComplete()
        {
            Debug.Log("Unpossession complete");
            animComplete = true;
            PossessionReleased?.Invoke();
            PossessedCharacter = null;

            unpossessionCallback?.Invoke();
            unpossessionCallback = null;
        }

        public void ReleaseCurrentPossession(bool repossession = false, Action callback = null)
        {
            if (PossessedCharacter == null)
            {
                return;
            }

            // Is this the characters own physical form? If not, enrage it.
            var isOwnPhysicalForm = (IPossessable)PhysicalForm == PossessedCharacter;
            PossessedCharacter.ReleasePossession(isOwnPhysicalForm);

            // Handle spirit form
            if (!repossession)
            {
                unpossessionCallback = callback;
                transform.position = GetPositionAfterPossession();
                ShowSpiritForm();
                //CameraSystem.Target = transform;
            }
            else
            {
                animComplete = false;
                StartCoroutine("CallAfterRepossession");
                //_animator.SetTrigger("Repossess");
            }
        }

        private Vector3 GetPositionAfterPossession()
        {
            return PossessedCharacter.Controller.transform.position - PossessedCharacter.Controller.transform.forward;
        }
    }
}
