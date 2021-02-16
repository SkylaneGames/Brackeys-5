using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerInput : MonoBehaviour
{
    CharacterMovement movement;
    public CinemachineFreeLook FreeLookCamera;
    FreeLookAddOn freeLookAddOn;
    public Camera sceneCamera;
    
    Rigidbody body;
    Vector3 translation;
    public float speed;
    //public float turnSpeed;
    float rotation;
    void Start(){
        movement = GetComponent<CharacterMovement>();
        freeLookAddOn = FreeLookCamera.GetComponent<FreeLookAddOn>();
        body = GetComponent<Rigidbody>();
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
            translation += Vector3.forward;
        }

        if (keyboard.sKey.isPressed){
            translation += -Vector3.forward;
        }

        if (keyboard.aKey.isPressed){
            translation += -Vector3.right;
        }

        if (keyboard.dKey.isPressed){
            translation += Vector3.right;
        }
        
        if(body.velocity.magnitude>0.5f){
            //var direction = new Vector3(transform.position.x - FreeLookCamera.transform.position.x, 0, transform.position.z - FreeLookCamera.transform.position.z);
            var direction = sceneCamera.transform.forward;
            direction.y=0;
            var resultVector = transform.position + direction;
            resultVector.y = 0f;
            //var newRotation = Quaternion.FromToRotation(transform.forward.normalized, resultVector.normalized);
            var newRotation = Quaternion.LookRotation(direction.normalized,Vector3.up);
            Debug.Log(newRotation.eulerAngles);
            //transform.LookAt(resultVector, Vector3.zero);
            translation = newRotation * translation;
        }
        
        movement.UpdateMovement(translation.normalized);
        translation = Vector3.zero;
    }

}
