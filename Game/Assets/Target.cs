using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IPuzzleElement
{
    public GameObject targetCenter;
    private Renderer targetRenderer;
    
    private bool isHit = false;

    void Awake(){
        targetRenderer = targetCenter.GetComponent<Renderer>();
    }
    
    private void OnTriggerEnter(Collider collider){
        if(collider.tag.Equals("Fireball")){
            isHit = true;
            targetRenderer.material.SetVector("_EmissionColor", new Vector4(0.0f,0.5f,0.0f,1f) * 4f);
            GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    public bool IsComplete(){
        return isHit;
    }

    public void SetColor(Vector4 color, float intensity)
    {
        GetComponentInChildren<ParticleSystem>().GetComponent<Renderer>().material.SetVector("_EmissionColor", color * intensity);
    }
}
