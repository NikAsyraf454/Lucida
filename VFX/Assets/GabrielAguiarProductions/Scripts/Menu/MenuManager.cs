using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject difficultyMenuPrefab;
    public GameObject pauseMenuPrefab;
    public GameObject winMenuPrefab;
    public GameObject loseMenuPrefab;
    public GameObject nextSectionPrefab;
    [SerializeField] private GameObject graphMenuPrefab;
    [SerializeField] private CSVReader cSVReader;
    [SerializeField] private CSVWriter cSVWriter;
    private PauseMenu pause;

    public bool gameEnded;

    string filename => $"{Application.persistentDataPath}/performance.csv";
    private string savePath => $"{Application.persistentDataPath}/save.txt";

    void Awake()
    {
        Instance = this;
        gameEnded = false;
        // Time.timeScale = 1;
    }

    void Start()
    {
        pause = GetComponent<PauseMenu>();
        if(!SaveManager.Instance.saveFileExist)
        {
            pause.HaltGame();
            ChooseDifficulty();
        }
        else
        {
            pause.ResumeGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseDifficulty()
    {
        difficultyMenuPrefab.SetActive(true);
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

    public void DifficultyEasy()
    {
        PlayerManager.Instance.SetDifficulty(PlayerManager.Difficulty.Easy);
        //reload enemy list
        pause.ResumeGame();
        difficultyMenuPrefab.SetActive(false);
    }

    public void DifficultyNormal()
    {
        PlayerManager.Instance.SetDifficulty(PlayerManager.Difficulty.Normal);
        //reload enemy list
        pause.ResumeGame();
        difficultyMenuPrefab.SetActive(false);
    }

    public void DifficultyHard()
    {
        PlayerManager.Instance.SetDifficulty(PlayerManager.Difficulty.Hard);
        //reload enemy list
        pause.ResumeGame();
        difficultyMenuPrefab.SetActive(false);
    }

    public void RestartGame()
    {
        File.Delete(savePath);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void PauseGame()
    {
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

    public void ButtonSectionUnlocked()
    {
        nextSectionPrefab.SetActive(true);
    }

    public void StartNextSection()
    {
        WaveManager.Instance.NextSection();
        nextSectionPrefab.SetActive(false);
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
