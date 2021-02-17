using Combat;
using UnityEngine;

public enum CharacterType
{
    Physical, Spirit
}

[RequireComponent(typeof(Possess_CharacterMovement))]
public abstract class CharacterController : MonoBehaviour
{
    private Possess_CharacterMovement _movementSystem;
    private CharacterInteraction _interactionSystem;
    private CombatSystem _combatSystem;

    public virtual Possess_CharacterMovement MovementSystem
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

    public abstract CharacterType CharacterType { get; }

    private bool isBusy;
    public virtual bool IsBusy
    {
        get { return isBusy; }
    }

    protected bool ShowHightlightsOriginal;
    public bool ShowHightlights = false;

    protected virtual void Awake()
    {
        _movementSystem = GetComponent<Possess_CharacterMovement>();
        _interactionSystem = GetComponentInChildren<CharacterInteraction>();
        _combatSystem = GetComponentInChildren<CombatSystem>();

        ShowHightlightsOriginal = ShowHightlights;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        InteractionSystem.UseHighlights = ShowHightlights;
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
