using Possession;

public class PhysicalController : CharacterController
{
    public override CharacterType CharacterType => CharacterType.Physical;

    private IPossessable Possessable;

    protected virtual void OnPossessed()
    {
        // Disable the script so the update sections will not be called
        this.enabled = false;

        // Set the show highlights value to the same as the character who is possessing this one.
        InteractionSystem.UseHighlights = Possessable.PossessingCharacter.Controller.ShowHightlights;
    }

    protected virtual void OnPossessionReleased()
    {
        MovementSystem.StopMoving();

        // Set the highlights values back to what it was before.
        InteractionSystem.UseHighlights = ShowHightlightsOriginal;

        // Re-enable the update sections
        this.enabled = true;
    }

    protected override void Awake()
    {
        base.Awake();

        Possessable = GetComponentInChildren<IPossessable>();

        if (Possessable != null)
        {
            Possessable.Possessed += OnPossessed;
            Possessable.PossessionReleased += OnPossessionReleased;
        }
    }
}
