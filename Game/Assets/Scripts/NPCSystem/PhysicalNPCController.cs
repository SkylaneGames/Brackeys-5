using System;
using Possession;
using UnityEngine;

namespace NPC
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicalNPCController : NPCController
    {
        private PhysicalController _physicalController;
        private Rigidbody _rigidbody;

        protected IPossessable Possessable => _physicalController.Possessable;
        protected Rigidbody Rigidbody => _rigidbody;

        protected override void Awake()
        {
            base.Awake();

            _physicalController = Controller as PhysicalController;
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected override void Start()
        {
            base.Start();

            Possessable.Possessed += OnPossessed;
            Possessable.PossessionReleased += OnUnpossessed;
        }

        private void OnPossessed()
        {
            enabled = false;
            Controller.MovementSystem.enabled = true;
            Rigidbody.isKinematic = false;
            NavMeshAgent.isStopped = true;
            NavMeshAgent.enabled = false;
        }

        private void OnUnpossessed()
        {
            enabled = true;
            Controller.MovementSystem.enabled = false;
            Rigidbody.isKinematic = true;
            NavMeshAgent.enabled = true;
        }
    }
}