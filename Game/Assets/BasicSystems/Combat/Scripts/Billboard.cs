using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

    void Awake()
    {
        if (target == null)
        {
            target = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + target.forward);
    }
}
