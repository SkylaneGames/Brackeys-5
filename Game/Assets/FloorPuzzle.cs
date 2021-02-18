using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPuzzle : MonoBehaviour
{
    public GameObject Door;

    private void OnTriggerEnter(Collider collider){
        
        if(collider.gameObject.tag.Equals("PuzzleBox")){
            Debug.Log("Box in circle");
            Door.GetComponent<Animator>().SetTrigger("Open");
        }
    }
}
