using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    Rigidbody body;
    Vector3 currentTranslation;
    CharacterAnimation animation;
    float rotation;

    void Awake(){
        body =  GetComponent<Rigidbody>();
        animation = GetComponent<CharacterAnimation>();
    }

    void FixedUpdate(){
        body.velocity = currentTranslation;
        transform.Rotate(0f, rotation, 0f, Space.Self);
        currentTranslation = Vector3.zero;
        rotation = 0f;
    }

    void LateUpdate(){
        if(body.velocity.magnitude>0.5f){
            animation.RunForward();
        }
        else{
            animation.StopMoving();
        }
    }
  
    public void UpdateMovement(Vector3 translation, float newRotation){
        currentTranslation = translation;
        rotation = newRotation;
    }
}
