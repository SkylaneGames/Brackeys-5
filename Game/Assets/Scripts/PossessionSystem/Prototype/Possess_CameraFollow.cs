using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possess_CameraFollow : MonoBehaviour
{
    public Transform Target = null;

    private Vector3 offset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (Target != null)
        {
            offset = transform.position - Target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            var newPos = Target.position + offset;

            transform.position = newPos;
        }
    }
}
