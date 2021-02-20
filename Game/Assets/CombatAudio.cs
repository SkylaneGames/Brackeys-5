using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAudio : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    public AudioClip[] AudioList;

    void Awake(){
        audioSource = GetComponentInParent<AudioSource>();
    }
    public void AttackSound(){
        audioSource.PlayOneShot(AudioList[0],0.5f);
    }

    public void HitSound(){
        audioSource.PlayOneShot(AudioList[1],0.2f);
    }


}
