using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public float smooth;
    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var result = transform.position + body.velocity.normalized;
        Quaternion targetRotation;
        if(body.velocity.magnitude > 0.5f){
            targetRotation = Quaternion.LookRotation(body.velocity.normalized, Vector3.up);
        }
        else{
            targetRotation = Quaternion.LookRotation(transform.parent.forward, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, smooth * Time.deltaTime) ;
    }
}
