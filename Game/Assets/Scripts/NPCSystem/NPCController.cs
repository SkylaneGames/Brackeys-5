using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

namespace NPC
{
    public enum NPCActivity
    {
        Idle, Moving, Attacking, Interacting
    }

    [RequireComponent(typeof(CharacterController))]
    public abstract class NPCController : MonoBehaviour
    {
        // Controller
        public virtual CharacterController Controller { get; private set; }

        // System Accessors
        protected CharacterVision VisionSystem => _visionSystem;
        protected virtual CombatSystem CombatSystem => Controller.CombatSystem;
        protected virtual HealthSystem HealthSystem => Controller.CombatSystem.HealthSystem;
        protected virtual CharacterInteraction InteractionSystem => Controller.InteractionSystem;

        // System instances native to NPC controller
        private CharacterVision _visionSystem;

        protected NPCActivity Status = NPCActivity.Idle;

        protected virtual void Awake()
        {
            _visionSystem = GetComponent<CharacterVision>();
            Controller = GetComponent<CharacterController>();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}
