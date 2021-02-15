using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    void Awake(){
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunForward(){
        animator.SetBool("Moving Forward", true);
    }

    public void StopMoving(){
        animator.SetBool("Moving Forward", false);
    }
}
