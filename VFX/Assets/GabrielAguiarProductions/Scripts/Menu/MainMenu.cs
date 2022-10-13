using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject SettingsMenuPrefab;
    private bool settingMenu;

    // Start is called before the first frame update
    void Start()
    {
        settingMenu = false;
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
            SettingsMenuPrefab.SetActive(true);
            settingMenu = true;
        }
        else
        {
            SettingsMenuPrefab.SetActive(false);
            settingMenu = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
