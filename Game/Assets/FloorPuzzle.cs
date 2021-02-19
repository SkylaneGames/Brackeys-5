using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPuzzle : MonoBehaviour, IPuzzleElement
{
    public GameObject puzzleBox;
    private ParticleSystem[] particles;
    private ParticleSystem completeParticles;
    private bool isComplete = false;

    void Awake(){
        particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem item in particles)
        {
            if(item.name.Equals("Particle System 2")){
                completeParticles = item;
            }
        }
    }

    private void OnTriggerEnter(Collider collider){
        
        if(collider.gameObject.tag.Equals("PuzzleBox")){
            isComplete = true;
            completeParticles.Play();
        }
    }

    private void OnTriggerExit(Collider collider){

        if(collider.gameObject.tag.Equals("PuzzleBox")){
            completeParticles.Stop();
        }
    }

    public bool IsComplete(){
        return isComplete;
    }

    public void SetColor(Vector4 color, float intensity)
    {
        Debug.Log("Settinbg floor puzzle color, count of particle systems = " + particles.Length);
        foreach(ParticleSystem particle in particles){
            particle.gameObject.GetComponent<Renderer>().material.SetVector("_EmissionColor", color * intensity);
        }
        puzzleBox.GetComponentInChildren<ParticleSystem>().gameObject.GetComponent<Renderer>().material.SetVector("_EmissionColor", color * intensity);
    }
}
