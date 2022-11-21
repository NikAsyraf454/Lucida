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
    private string savePath => $"{Application.persistentDataPath}/save.txt";

    void Awake()
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
        if(gameEnded) { return; }
        pauseMenuPrefab.SetActive(false);
        gameEnded = true;
        Time.timeScale = 0;
        loseMenuPrefab.SetActive(true);
        LoadCSV();
        SaveManager.Instance.DeleteSave();
    }

    public void PlayerWin()
    {
        if(gameEnded) { return; }
        pauseMenuPrefab.SetActive(false);
        gameEnded = true;
        Time.timeScale = 0;
        winMenuPrefab.SetActive(true);
        LoadCSV();
        SaveManager.Instance.DeleteSave();
    }

    public void RestartGame()
    {
        File.Delete(savePath);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void PauseGame()
    {
        PauseMenu pause = GetComponent<PauseMenu>();
        pause.PauseGame();
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


    public int[] GetCSVColumn(int j)
    {
        ScoreList temp = cSVReader.ReadCSV();
        int[] arr = new int[temp.scores.Length];

        for(int i = 0; i<temp.scores.Length; i++)
        {
            switch(j)
            {
                case 1:
                    arr[i] = temp.scores[i].health;
                    break;
                case 2:
                    arr[i] = temp.scores[i].level;
                    break;
                case 3:
                    arr[i] = temp.scores[i].score;
                    break;
            }
        }
        
        return arr;
    }

        public List<int> GetCSVColumnList(int j)
    {
        ScoreList temp = cSVReader.ReadCSV();
        List<int> list = new List<int>();

        for(int i = 0; i<temp.scores.Length; i++)
        {
            switch(j)
            {
                case 1:
                    list.Add(temp.scores[i].health);
                    break;
                case 2:
                    list.Add(temp.scores[i].level);
                    break;
                case 3:
                    list.Add(temp.scores[i].score);
                    break;
            }
            
        }
        
        return list;
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
