using System.Collections;
using System.Collections.Generic;
using CoreSystems.MenuSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private PauseMenu PauseMenu;

    void Awake()
    {
        PauseMenu = FindObjectOfType<PauseMenu>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (PauseMenu.IsPaused)
            {
                PauseMenu.UnpauseGame();
            }
            else
            {
                PauseMenu.PauseGame();
            }
        }
    }
}
