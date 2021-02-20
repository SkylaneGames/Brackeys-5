using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public enum NPCActivity
    {
        Idle, Moving, Attacking, Interacting
    }

    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public abstract class NPCController : MonoBehaviour
    {
        // Controller
        public virtual CharacterController Controller { get; private set; }

        // System Accessors
        protected CharacterVision VisionSystem => _visionSystem;
        protected NavMeshAgent NavMeshAgent => _navMeshAgent;
        protected Animator Animator => _animator;
        protected virtual CombatSystem CombatSystem => Controller.CombatSystem;
        protected virtual HealthSystem HealthSystem => Controller.CombatSystem.HealthSystem;
        protected virtual CharacterInteraction InteractionSystem => Controller.InteractionSystem;

        // System instances native to NPC controller
        private CharacterVision _visionSystem;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        public float activityDelay = 1f;
        private float wait = 1f;

        public float AttackRange = 2f;

        protected NPCActivity Status = NPCActivity.Idle;

        protected virtual void Awake()
        {
            _visionSystem = GetComponentInChildren<CharacterVision>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            Controller = GetComponent<CharacterController>();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            Controller.MovementSystem.enabled = false;
            HealthSystem.CharacterKilled += OnDeath;
        }

        private void OnDeath()
        {
            this.enabled = false;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (Controller.IsBusy)
            {
                NavMeshAgent.isStopped = true;
            }
            else
            {
                NavMeshAgent.isStopped = false;
            }

            PickActivity();

            if (wait > 0)
            {
                wait -= Time.deltaTime;
            }
        }

        protected virtual void FixedUpdate()
        {
            Animator.SetBool("Moving Forward", NavMeshAgent.velocity.sqrMagnitude > 0.5f);
        }

        protected virtual void PickActivity()
        {
            // Debug.Log($"[{name}] Picking activity");

            var picked = CheckForPlayer();
            if (!picked && wait <= 0)
            {
                // Debug.Log($"[{name}] Moving randomly");
                NavMeshAgent.SetDestination(GetRandomPosition(5));
                wait = activityDelay;
            }
        }

        protected abstract bool AttackPlayer(CharacterController player);

        private bool CheckForPlayer()
        {
            // Debug.Log($"[{name}] {VisionSystem.Characters.Count()} characters in view");
            // Get the any characters without a NPC Controller, this will be the player or the player's physical form
            var player = VisionSystem.Characters.FirstOrDefault(p => p.GetComponent<NPCController>() == null);
            if (player != null)
            {
                if (AttackPlayer(player))
                {
                    return true;
                }
                
                // Debug.Log($"[{name}] Moving to player");
                var toPlayer = player.transform.position - transform.position;
                var direction = toPlayer.normalized;
                var moveTo = player.transform.position - direction; // stop just before getting to the player
                NavMeshAgent.SetDestination(moveTo);
                return true;
            }

            return false;
        }

        protected Vector3 GetRandomPosition(float maxRadius)
        {
            var direction = Quaternion.AngleAxis(Random.value * 360, Vector3.up) * transform.forward;

            var newPos = transform.position + direction * maxRadius * Random.value;

            return newPos;

        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);

        }
    }
}
