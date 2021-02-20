using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public List<GameObject> PuzzleElements;
    public GameObject Door;

    private bool puzzleComplete = false;
    private bool DoorOpen = false;

    public Vector4 PuzzleColor = new Vector4(1,1,1,1);
    public float Intensity = 1.0f;

    void Start(){
        Debug.Log("Amount of puzzles = " + PuzzleElements.Capacity);
        // Set color of particles for this puzzle
        foreach (GameObject puzzle in PuzzleElements)
        {
            puzzle.GetComponentInChildren<IPuzzleElement>().SetColor(PuzzleColor, Intensity);
        }

        // Set color of particles on the door
        Door.GetComponentInChildren<ParticleSystem>().GetComponent<Renderer>().material.SetVector("_EmissionColor", PuzzleColor * Intensity);
    }

    void Update(){
        if(!puzzleComplete){
            puzzleComplete = true;
            foreach (GameObject puzzle in PuzzleElements)
            {
                if(!puzzle.GetComponentInChildren<IPuzzleElement>().IsComplete()){
                    puzzleComplete = false;
                }
            }
        }
        else{
            if(!DoorOpen){
                Door.GetComponent<Animator>().SetTrigger("Open");
                Door.GetComponentInChildren<ParticleSystem>().Stop();
                Door.GetComponent<AudioSource>().PlayOneShot(Door.GetComponent<AudioSource>().clip);
                DoorOpen = true;
            }
        }
    }
}
