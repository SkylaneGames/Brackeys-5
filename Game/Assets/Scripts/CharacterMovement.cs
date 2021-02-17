using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    public float smooth;
    public float Speed = 5f;
    Rigidbody body;
    Vector3 currentTranslation;
    CharacterAnimation _animation;
    float rotation;

    void Awake(){
        body =  GetComponent<Rigidbody>();
        _animation = GetComponent<CharacterAnimation>();
    }


    void Update(){
        
    }
    void FixedUpdate(){
        body.velocity = currentTranslation;
        if(body.velocity.magnitude>0.5f){
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(body.velocity.x, 0, body.velocity.z).normalized), smooth * Time.deltaTime) ;
            //transform.rotation = Quaternion.LookRotation(new Vector3(body.velocity.x, 0, body.velocity.z).normalized);
        }
        //transform.Rotate(0f, rotation, 0f, Space.Self);
        //currentTranslation = Vector3.zero;
        //rotation = 0f;
    }

    void LateUpdate(){
        if(body.velocity.magnitude>0.5f){
            _animation.RunForward();
        }
        else{
            _animation.StopMoving();
        }

        
    }
  
    public void UpdateMovement(Vector3 translation){
        currentTranslation = translation * Speed;
        //rotation = newRotation;
    }
}
