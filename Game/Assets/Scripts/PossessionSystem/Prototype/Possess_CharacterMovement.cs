using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possess_CharacterMovement : MonoBehaviour
{
    float currentSpeed = 0f;
    float currentRotation = 0f;

    public float Speed = 3f;
    public float AngularSpeed = 360f;

    void FixedUpdate()
    {
        transform.Rotate(transform.up, currentRotation * Time.deltaTime);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    public void ProcessInput(float dForward, float dRotation)
    {
        currentSpeed = dForward * Speed;
        currentRotation = dRotation * AngularSpeed;
    }

    public void StopMoving()
    {
        currentSpeed = 0f;
        currentRotation = 0f;
    }
}
