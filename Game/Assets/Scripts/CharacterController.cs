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
    private CombatSystem _combatSystem;
    private Animator _animator;

    

    public virtual CharacterMovement MovementSystem
    {
        get { return _movementSystem; }
    }

    public virtual CharacterInteraction InteractionSystem
    {
        get { return _interactionSystem; }
    }

    public virtual CombatSystem CombatSystem
    {
        get { return _combatSystem; }
    }

    public virtual Animator Animator
    {
        get { return _animator; }
    }

    public abstract CharacterType CharacterType { get; }

    private bool isBusy;
    public virtual bool IsBusy
    {
        get { return isBusy; }
    }

    public bool ShowHightlights = false;

    protected virtual void Awake()
    {
        _movementSystem = GetComponent<CharacterMovement>();
        _interactionSystem = GetComponentInChildren<CharacterInteraction>();
        _combatSystem = GetComponentInChildren<CombatSystem>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void OnCharacterKilled()
    {
        this.enabled = false;
        _animator.SetTrigger("Dead");
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InteractionSystem.UseHighlights = ShowHightlights;
        CombatSystem.HealthSystem.CharacterKilled += OnCharacterKilled;
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
