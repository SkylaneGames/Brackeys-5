using Combat;
using Possession;

public class SpiritController : CharacterController
{
    private PossessionSystem _possessionSystem;

    public PossessionSystem PossessionSystem
    {
        get { return _possessionSystem; }
    }

    public override Possess_CharacterMovement MovementSystem
    {
        get
        {
            return _possessionSystem.IsPossessing ?
                _possessionSystem
                    .PossessedCharacter
                    .Transform
                    .GetComponent<Possess_CharacterMovement>()
                : base.MovementSystem;
        }
    }

    public override CharacterInteraction InteractionSystem
    {
        get
        {
            return _possessionSystem.IsPossessing ?
                _possessionSystem
                    .PossessedCharacter
                    .Transform
                    .GetComponentInChildren<CharacterInteraction>()
                : base.InteractionSystem;
        }
    }

    public override CombatSystem CombatSystem
    {
        get
        {
            return _possessionSystem.IsPossessing ?
                _possessionSystem
                    .PossessedCharacter
                    .Transform
                    .GetComponentInChildren<CombatSystem>()
                : base.CombatSystem;
        }
    }

    public override bool IsBusy => base.IsBusy || InteractionSystem.IsInteracting || isUnpossessing;

    public override CharacterType CharacterType => CharacterType.Spirit;

    private bool isUnpossessing = false;

    protected override void Awake()
    {
        base.Awake();

        _possessionSystem = GetComponent<PossessionSystem>();

        _possessionSystem.CharacterPossessed += () => base.InteractionSystem.UseHighlights = false;
        _possessionSystem.PossessionReleased += () => base.InteractionSystem.UseHighlights = true;
    }

    protected void UnPossess()
    {
        isUnpossessing = true;
        _possessionSystem.ReleaseCurrentPossession(callback: () => isUnpossessing = false);
    }
}
