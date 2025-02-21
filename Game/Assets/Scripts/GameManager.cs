using System.Collections;
using System.Collections.Generic;
using CoreSystems.MenuSystem;
using CoreSystems.TransitionSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private PauseMenu PauseMenu;
    private LevelLoader LevelLoader;

    public GameObject GameOverText;
    public GameObject BodyDiedText;
    public GameObject WinText;

    void Awake()
    {
        PauseMenu = FindObjectOfType<PauseMenu>();
        LevelLoader = FindObjectOfType<LevelLoader>();
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

    public void PlayerDied()
    {
        StartCoroutine(GameOver());
        // Slow down time maybe?

        // Fade in "Game Over" over a few seconds then go back to the main menu.
    }

    public void PhysicalBodyKilled()
    {
        StartCoroutine(BodyDied());
        StartCoroutine(GameOver());
        // Slow down time maybe?

        // Fade in "Game Over" over a few seconds then go back to the main menu.
    }

    private IEnumerator BodyDied()
    {
        yield return new WaitForSeconds(1f);
        BodyDiedText.SetActive(true);
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        GameOverText.SetActive(true);
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.None;
        LevelLoader.LoadLevel(Level.Menu);
    }

    public void PlayerExited()
    {
        StartCoroutine(PlayerWins());
        // Fade in "Congradulations you escaped" (or something) then go back to the main menu.
    }

    private IEnumerator PlayerWins()
    {
        yield return new WaitForSeconds(1f);
        WinText.SetActive(true);
        yield return new WaitForSeconds(2f);

        Cursor.lockState = CursorLockMode.None;
        LevelLoader.LoadLevel(Level.Menu);
    }
}
