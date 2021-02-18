using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineFreeLook))]
public class FreeLookAddOn : MonoBehaviour
{
    public float LookSpeedX = 1.0f;
    public float LookSpeedY = 1.0f;
    public bool InvertY = false;
    private CinemachineFreeLook freeLookComponent;

    public void Start(){
        freeLookComponent = GetComponent<CinemachineFreeLook>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputAction.CallbackContext context){
        //Normalize the vector to have an uniform vector in whichever form it came from (I.E Gamepad, mouse, etc)
        Vector2 lookMovement = context.ReadValue<Vector2>().normalized;
        lookMovement.y = InvertY ? -lookMovement.y : lookMovement.y;

        // This is because X axis is only contains between -180 and 180 instead of 0 and 1 like the Y axis
        lookMovement.x = lookMovement.x * 180f; 

        //Ajust axis values using look speed and Time.deltaTime so the look doesn't go faster if there is more FPS
        freeLookComponent.m_XAxis.Value += lookMovement.x * LookSpeedX * Time.fixedDeltaTime;
        freeLookComponent.m_YAxis.Value += lookMovement.y * LookSpeedY * Time.fixedDeltaTime;
    }
}
