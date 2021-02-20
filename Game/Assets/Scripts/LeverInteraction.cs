using System;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LeverInteraction : MonoBehaviour, IInteractable, IPuzzleElement
{
    public string Name => name;

    public Transform Transform => transform.parent;

    public InteractionHighlight HighlightObject { get; protected set; }
    //public GameObject Door;

    Animation LeverAnimation;

    private bool LeverActivated = false;
    public ParticleSystem particles;

    private bool ElementComplete = false;

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
        GetComponentInParent<AudioSource>().PlayOneShot(GetComponentInParent<AudioSource>().clip);
        LeverActivated = true;
        ElementComplete = true;
        particles.Stop();
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

    public bool IsComplete()
    {
        return ElementComplete;
    }

    public void SetColor(Vector4 color, float intensity)
    {
        particles.gameObject.GetComponent<Renderer>().material.SetVector("_EmissionColor", color * intensity);
    }
}
