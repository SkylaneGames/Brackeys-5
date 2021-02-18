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

        public IPossessable PossessedCharacter = null;

        public Possessable PhysicalForm = null;

        //private Possess_CameraFollow CameraSystem;

        [SerializeField]
        [Range(0,1)]
        private float possessionPower = 0.4f;

        public float PoessessionPower => possessionPower;

        public event Action CharacterPossessed;
        public event Action PossessionReleased;

        protected Animator _animator;

        public CharacterController Controller { get; set; }

        private bool animComplete = true;

        private PossessionArgs nextPossession = null;

        private Action unpossessionCallback = null;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            Controller = GetComponent<CharacterController>();

            //CameraSystem = FindObjectOfType<Possess_CameraFollow>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (PossessedCharacter != null)
            {
                HideSpiritForm();
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
            StartCoroutine("CallAfterUnpossession");
            //_animator.SetTrigger("Unpossess");
        }

        private void HideSpiritForm()
        {
            animComplete = false;
            StartCoroutine("CallAfterPossession");
            //_animator.SetTrigger("Possess");
            
        }

        private IEnumerator CallAfterPossession(){
            yield return new WaitForSeconds(1);
            PossessionComplete();
        }

        private IEnumerator CallAfterRepossession(){
            yield return new WaitForSeconds(1);
            RepossessionComplete();
        }

        private IEnumerator CallAfterUnpossession(){
            yield return new WaitForSeconds(1);
            UnpossessionComplete();
        }

        private void OnPossessionComplete()
        {
            CharacterPossessed?.Invoke();
            
            PossessedCharacter = nextPossession.character;
            //CameraSystem.Target = PossessedCharacter.Controller.transform;

            nextPossession.callback?.Invoke();
            nextPossession = null;
        }

        public void PossessionComplete()
        {
            Debug.Log("Possession complete");
            animComplete = true;
            OnPossessionComplete();
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
            PossessionReleased?.Invoke();
            
            // Handle spirit form
            if (!repossession)
            {
                unpossessionCallback = callback;
                transform.position = GetPositionAfterPossession();
                ShowSpiritForm();
                //CameraSystem.Target = transform;
                PossessedCharacter = null;
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
