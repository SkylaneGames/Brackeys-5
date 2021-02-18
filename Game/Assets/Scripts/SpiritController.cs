using Combat;
using Possession;
using UnityEngine;

[RequireComponent(typeof(PossessionSystem))]
public class SpiritController : CharacterController
{
    private PossessionSystem _possessionSystem;
    private AbilitySystem _abilitySystem;
    private MeshRenderer[] _meshRenderers;

    [SerializeField]
    private GameObject body;

    public PossessionSystem PossessionSystem
    {
        get { return _possessionSystem; }
    }

    public AbilitySystem AbilitySystem
    {
        get { return _abilitySystem; }
    }

    public override CharacterMovement MovementSystem
    {
        get
        {
            return _possessionSystem.IsPossessing ?
                _possessionSystem.PossessedCharacter.Controller.MovementSystem
                : base.MovementSystem;
        }
    }

    public override CharacterInteraction InteractionSystem
    {
        get
        {
            return _possessionSystem.IsPossessing ?
                _possessionSystem.PossessedCharacter.Controller.InteractionSystem
                : base.InteractionSystem;
        }
    }

    public override CombatSystem CombatSystem
    {
        get
        {
            return _possessionSystem.IsPossessing ?
                _possessionSystem.PossessedCharacter.Controller.CombatSystem
                : base.CombatSystem;
        }
    }

    public override bool IsBusy => base.IsBusy
        || InteractionSystem.IsInteracting
        || isUnpossessing
        || (AbilitySystem?.IsCasting ?? false);

    public override CharacterType CharacterType => CharacterType.Spirit;

    private bool isUnpossessing = false;

    protected override void Awake()
    {
        base.Awake();

        _possessionSystem = GetComponent<PossessionSystem>();
        _abilitySystem = GetComponentInChildren<AbilitySystem>();
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    protected override void Start()
    {
        base.InteractionSystem.UseHighlights = ShowHightlights;

        _possessionSystem.CharacterPossessed += () => base.InteractionSystem.UseHighlights = false;
        _possessionSystem.PossessionReleased += () => base.InteractionSystem.UseHighlights = ShowHightlights;
    }

    protected void UnPossess()
    {
        isUnpossessing = true;
        _possessionSystem.ReleaseCurrentPossession(callback: () => isUnpossessing = false);
    }

    public void SetVisibility(bool visible)
    {
        // body.SetActive(visible);
        foreach (var mesh in _meshRenderers)
        {
            mesh.enabled = visible;
        }
    }
}
