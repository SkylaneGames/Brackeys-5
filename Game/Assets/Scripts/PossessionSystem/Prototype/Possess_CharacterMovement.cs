using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possess_CharacterMovement : MonoBehaviour
{
    float currentSpeed = 0f;
    float currentRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        transform.Rotate(transform.up, currentRotation * Time.deltaTime);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    public void ProcessInput(float speed, float rotation)
    {
        currentSpeed = speed;
        currentRotation = rotation;
    }

    public void StopMoving()
    {
        currentSpeed = 0f;
        currentRotation = 0f;
    }
}
