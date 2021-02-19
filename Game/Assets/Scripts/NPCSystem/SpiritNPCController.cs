using System.Collections;
using System.Collections.Generic;
using Combat;
using Possession;
using UnityEngine;

namespace NPC
{
    public class SpiritNPCController : NPCController
    {
        private SpiritController _spiritController;
        protected virtual PossessionSystem PossessionSystem => _spiritController.PossessionSystem;

        public override CharacterController Controller => PossessionSystem.IsPossessing ?
            PossessionSystem.PossessedCharacter.Controller
            : base.Controller;

        protected override void Awake()
        {
            base.Awake();

            _spiritController = base.Controller as SpiritController;
        }
    }
}
