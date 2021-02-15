using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    CharacterMovement movement;
    
    Vector3 translation;
    public float speed;
    public float turnSpeed;
    float rotation;
    void Start(){
        movement = GetComponent<CharacterMovement>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    void FixedUpdate(){
        var keyboard = Keyboard.current;
        if (keyboard == null) {
            return;
        }
        var mouse = Mouse.current;
        if(mouse == null){
            return;
        }

        translation = Vector3.zero;
        if (keyboard.wKey.isPressed){
            translation += transform.forward*speed;
        }

        if (keyboard.sKey.isPressed){
            translation += transform.forward*-speed;
        }

        if (keyboard.aKey.isPressed){
            translation += transform.right*-speed;
        }

        if (keyboard.dKey.isPressed){
            translation += transform.right*speed;
        }

        //if (mouse.rightButton.isPressed){
            rotation = mouse.delta.x.ReadValue()* turnSpeed;
        // }
        // else{
        //     rotation = 0;
        // }



        movement.UpdateMovement(translation,rotation);
        translation = Vector3.zero;
    }

}
