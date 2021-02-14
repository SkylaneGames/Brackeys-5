using System;
using System.Collections;
using System.Collections.Generic;
using Possession;
using Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Possess_CharacterMovement))]
public class PossessionPrototype_PlayerController : MonoBehaviour
{
    private Possess_CharacterMovement _movementSystem;
    private CharacterInteraction _interactionSystem;
    private PossessionSystem _possessionSystem;

    public Possess_CharacterMovement MovementSystem
    {
        get
        {
            return _possessionSystem.IsPossessing ? _possessionSystem.PossessedCharacter.MovementSystem : _movementSystem;
        }
    }

    public CharacterInteraction InteractionSystem
    {
        get
        {
            return _possessionSystem.IsPossessing ? _possessionSystem.PossessedCharacter.InteractionSystem : _interactionSystem;
        }
    }

    public float Speed = 2f;
    public float angularSpeed = 2f;

    void Awake()
    {
        _movementSystem = GetComponent<Possess_CharacterMovement>();
        _interactionSystem = GetComponentInChildren<CharacterInteraction>();
        _possessionSystem = GetComponent<PossessionSystem>();

        _possessionSystem.CharacterPossessed += () => _interactionSystem.UseHighlights = false;
        _possessionSystem.PossessionReleased += () => _interactionSystem.UseHighlights = true;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        var speed = 0f;
        if (Keyboard.current.wKey.isPressed)
        {
            speed = Speed;
        }

        var rotation = 0f;
        if (Keyboard.current.aKey.isPressed)
        {
            rotation -= angularSpeed;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            rotation += angularSpeed;
        }

        MovementSystem.ProcessInput(speed, rotation);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            _possessionSystem.ReleaseCurrentPossession();
        }
    }

    public void Interact()
    {
        if (InteractionSystem.LastInteractable != null)
        {
            Debug.Log($"Interacting with {InteractionSystem.LastInteractable.Name}");
            InteractionSystem.LastInteractable.Interact(gameObject);
        }
    }
}
