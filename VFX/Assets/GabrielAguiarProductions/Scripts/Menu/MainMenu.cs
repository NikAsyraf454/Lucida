using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private GameObject settingsMenuPrefab;
    [SerializeField] private GameObject startConfirmationPrefab;
    [SerializeField] private GameObject quitConfirmationPrefab;
    private bool settingMenu;
    private bool startConfirmation;
    private bool quitConfirmation;

    private string savePath => $"{Application.persistentDataPath}/save.txt";

    // Start is called before the first frame update
    void Start()
    {
        settingMenu = false;
        if(File.Exists(savePath)) 
        {
            continueBtn.interactable = true; 
            startBtn.onClick.AddListener(delegate{StartConfirmation();});
        } 
        else 
        {
            continueBtn.interactable = false; 
            startBtn.onClick.AddListener(delegate{StartGame();});
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        //playerprefs, set continue = true;
    }

    public void SettingsMenu()
    {
        if(!settingMenu)
        {
            settingsMenuPrefab.SetActive(true);
            settingMenu = true;
        }
        else
        {
            settingsMenuPrefab.SetActive(false);
            settingMenu = false;
        }
    }

    public void StartConfirmation()
    {
        if(!startConfirmation)
        {
            startConfirmationPrefab.SetActive(true);
            startConfirmation = true;
        }
        else
        {
            startConfirmationPrefab.SetActive(false);
            startConfirmation = false;
        }
    }

    public void StartConfirmed()
    {
        File.Delete(savePath);
        StartGame();
    }

    public void QuitConfirmation()
    {
        if(!quitConfirmation)
        {
            quitConfirmationPrefab.SetActive(true);
            quitConfirmation = true;
        }
        else
        {
            quitConfirmationPrefab.SetActive(false);
            quitConfirmation = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
