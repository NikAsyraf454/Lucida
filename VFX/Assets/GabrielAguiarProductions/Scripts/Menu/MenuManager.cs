using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject pauseMenuPrefab;
    public GameObject winMenuPrefab;
    public GameObject loseMenuPrefab;
    [SerializeField] private GameObject graphMenuPrefab;
    [SerializeField] private CSVReader cSVReader;
    [SerializeField] private CSVWriter cSVWriter;

    public bool gameEnded;

    string filename => $"{Application.persistentDataPath}/performance.csv";

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
        LoadCSV();
    }

    public void PlayerWin()
    {
        gameEnded = true;
        Time.timeScale = 0;
        winMenuPrefab.SetActive(true);
        LoadCSV();
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

    [ContextMenu("CSV")]
    public void LoadCSV()
    {
        if(!File.Exists(filename)) { cSVWriter.WriteEmptyCSV(); }

        cSVWriter.WriteCSV(cSVReader.ReadCSV());
        
    }
}

[System.Serializable]
public class Score
{
    public int health;
    public int level;
    public int score;
}

[System.Serializable]
public class ScoreList
{
    public Score[] scores;
}
