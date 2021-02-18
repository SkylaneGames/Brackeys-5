using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LeverInteraction : MonoBehaviour, IInteractable
{
    public string Name => name;

    public Transform Transform => transform.parent;

    public InteractionHighlight HighlightObject { get; protected set; }

    public GameObject Door;

    Animation LeverAnimation;

    private bool LeverActivated = false;

    public void Awake(){
        HighlightObject = GetComponentInChildren<InteractionHighlight>();
        LeverAnimation = GetComponentInParent<Animation>();
    }
    public bool CanInteract(CharacterController interacter)
    {
        return !LeverActivated && interacter.CharacterType == CharacterType.Physical;
    }

    public void Interact(CharacterController interacter, Action callback = null)
    {
        LeverAnimation.Play();
        LeverActivated = true;
        Door.GetComponent<Animator>().SetTrigger("Open");
        //Door.GetComponent<BoxCollider>().enabled = false;
        RemoveHighlight();
        callback?.Invoke();
    }
    public void Highlight()
    {
        HighlightObject?.Show();
    }

    public void RemoveHighlight()
    {
        HighlightObject?.Hide();
    }
}
