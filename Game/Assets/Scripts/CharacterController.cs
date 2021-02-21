using Combat;
using UnityEngine;

public enum CharacterType
{
    Physical, Spirit
}

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(Animator))]
public abstract class CharacterController : MonoBehaviour
{
    private CharacterMovement _movementSystem;
    private CharacterInteraction _interactionSystem;
    protected CombatSystem _combatSystem;
    private Animator _animator;
    private AudioSource _audioSource;
    private ParticleSystem _particleSystem;

    public virtual CharacterMovement MovementSystem => _movementSystem;
    public virtual CharacterInteraction InteractionSystem => _interactionSystem;
    public virtual CombatSystem CombatSystem => _combatSystem;
    public virtual Animator Animator => _animator;

    public abstract CharacterType CharacterType { get; }

    private Collider _bodyCollider;

    public virtual bool IsBusy => InteractionSystem.IsInteracting
        || CombatSystem.IsAttacking;

    public bool ShowHightlights = false;

    protected virtual void Awake()
    {
        _movementSystem = GetComponent<CharacterMovement>();
        _interactionSystem = GetComponentInChildren<CharacterInteraction>();
        _combatSystem = GetComponentInChildren<CombatSystem>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _bodyCollider = transform.GetChild(0).GetComponent<Collider>();
    }

    protected virtual void OnCharacterKilled()
    {
        this.enabled = false;
        _animator.SetTrigger("Dead");
        _audioSource.Play();
        _bodyCollider.enabled = false;
        if(_particleSystem != null) {
            _particleSystem.Play();
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InteractionSystem.UseHighlights = ShowHightlights;
        _combatSystem.HealthSystem.CharacterKilled += OnCharacterKilled;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public void Interact()
    {
        InteractionSystem.Interact();
    }
}
