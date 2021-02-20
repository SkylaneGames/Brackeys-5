using System.Collections;
using System.Collections.Generic;
using Possession;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitPortal : MonoBehaviour
{
    private GameManager GameManager;

    void Awake()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        var player = collider.GetComponentInParent<PlayerController>();
        
        if (player != null && player.PossessionSystem.PhysicalForm == (Possessable)player.PossessionSystem.PossessedCharacter)
        {
            player.OnWin();
            Debug.Log($"Hit something: {collider.gameObject.name}");
            GameManager.PlayerExited();
        }
    }
}
