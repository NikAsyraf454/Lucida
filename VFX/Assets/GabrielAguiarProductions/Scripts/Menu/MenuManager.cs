using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject pauseMenuPrefab;
    public GameObject winMenuPrefab;
    public GameObject loseMenuPrefab;
    [SerializeField] private GameObject graphMenuPrefab;

    public bool gameEnded;

    void Start()
    {
        Instance = this;
        gameEnded = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerLose()
    {
        gameEnded = true;
        Time.timeScale = 0;
        loseMenuPrefab.SetActive(true);
    }

    public void PlayerWin()
    {
        gameEnded = true;
        Time.timeScale = 0;
        winMenuPrefab.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void GraphMenu()
    {
        graphMenuPrefab.SetActive(true);
    }

    public void CloseGraph()
    {
        graphMenuPrefab.SetActive(false);
    }
}
