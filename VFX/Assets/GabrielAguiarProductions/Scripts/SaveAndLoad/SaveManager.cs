using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public bool saveFileExist = false;
    private string SavePath => $"{Application.persistentDataPath}/save.txt";

    void Awake()
    {
        Instance = this;
        if(File.Exists(SavePath))
        {
            saveFileExist=true;
            LoadDelay(0.2f); 
        }
    }

    // [ContextMenu("Save")]
    public void Save()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }

    // [ContextMenu("Load")]
    public void Load()
    {
        var state = LoadFile();
        RestoreState(state);
    }

    public void DeleteSave()
    {
        File.Delete(SavePath);
    }

    private Dictionary<string, object> LoadFile()
    {
        if(!File.Exists(SavePath))
        {
            return new Dictionary<string, object>();
        }

        using(FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void SaveFile(object state)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach(var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id]= saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach(var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if(state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }

    void LoadDelay(float delayTime)
    {
        StartCoroutine(DelayLoading(delayTime));
    }
    
    IEnumerator DelayLoading(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        
        yield return new WaitForSeconds(delayTime);
        MenuManager.Instance.PauseGame();
        Load();
        //Do the action after the delay time has finished.
    }

    public bool checkSaveFile()
    {
        if(File.Exists(SavePath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
