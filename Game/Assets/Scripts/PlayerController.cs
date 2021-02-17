using UnityEngine;
using UnityEngine.InputSystem;
using Combat;

[RequireComponent(typeof(Possess_CharacterMovement))]
public class PlayerController : SpiritController
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!IsBusy)
        {
            ProcessInput();
        }
    }

    void ProcessInput()
    {
        var speed = 0f;
        if (Keyboard.current.wKey.isPressed)
        {
            speed = 1f;
        }

        var rotation = 0f;
        if (Keyboard.current.aKey.isPressed)
        {
            rotation -= 1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            rotation += 1f;
        }

        MovementSystem.ProcessInput(speed, rotation);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CombatSystem.Attack();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            UnPossess();
        }
    }
}
