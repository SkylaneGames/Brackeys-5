using UnityEngine;
using UnityEngine.InputSystem;
using Combat;

public class PlayerController : SpiritController
{
    private Rigidbody rb;
    public Camera sceneCamera;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!IsBusy)
        {
            ProcessInput();
        }
        else
        {
            MovementSystem.StopMoving();
        }
    }

    void ProcessInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }
        var mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }
        var translation = Vector3.zero;
        if (keyboard.wKey.isPressed)
        {
            translation += Vector3.forward;
        }

        if (keyboard.sKey.isPressed)
        {
            translation += -Vector3.forward;
        }

        if (keyboard.aKey.isPressed)
        {
            translation += -Vector3.right;
        }

        if (keyboard.dKey.isPressed)
        {
            translation += Vector3.right;
        }

        var direction = sceneCamera.transform.forward;
        direction.y = 0;
        var newRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        translation = newRotation * translation;

        MovementSystem.UpdateMovement(translation.normalized);

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

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            AbilitySystem.Use(0);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            AbilitySystem.Use(1);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            AbilitySystem.Use(2);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            AbilitySystem.Use(3);
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            AbilitySystem.Use(4);
        }
    }
}
