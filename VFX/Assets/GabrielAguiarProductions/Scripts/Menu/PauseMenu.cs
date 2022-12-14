using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // [SerializeField] private GameObject pauseMenuPrefab;
    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        // ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(MenuManager.Instance.gameEnded){return;}

            if(isPaused ){ ResumeGame(); return; }
            PauseGame();
        }
    }

    public void PauseGame()         //pause game and activate pause menu
    {
        Time.timeScale = 0;
        MenuManager.Instance.pauseMenuPrefab.SetActive(true);
        isPaused = true;
    }

    public void HaltGame()         //pause game only
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        MenuManager.Instance.pauseMenuPrefab.SetActive(false);
        isPaused = false;
    }

    // public void QuitLevel(){}
    // public void PauseOption(){}


}
