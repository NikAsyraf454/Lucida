using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveable
{

    [SerializeField] private int currentPlayerHealth = 0;
    [SerializeField] private int maxPlayerHealth;

    [SerializeField] private int currentPlayerResources = 0;
    [SerializeField] private int maxPlayerResources;

    [SerializeField] private int currentPlayerScore = 0;
    [SerializeField] private int maxPlayerScore;

    [SerializeField] private int resourceMultiplier = 1;

    public event Action<int> ClientOnResourcesUpdated;
    public event Action<int> ClientOnPlayerHealthUpdated;
    public event Action<int> ClientOnScoreUpdated;


    // Start is called before the first frame update
    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        ClientHandlePlayerHealthUpdated(0, currentPlayerHealth);
        currentPlayerResources = maxPlayerResources;
        ClientHandleResourcesUpdated(0, currentPlayerResources);
        currentPlayerScore = maxPlayerScore;
        ClientHandleScoreUpdated(0, currentPlayerScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReducePlayerHealth()
    {
        int temp = currentPlayerHealth;
        currentPlayerHealth--;
        ClientHandlePlayerHealthUpdated(temp, currentPlayerHealth);
        //update UI playerhealth display
    }

    public void IncreaseResource(int resourceAmount)
    {
        int temp = currentPlayerResources;
        currentPlayerResources += resourceAmount * resourceMultiplier;
        ClientHandleResourcesUpdated(temp, currentPlayerResources);

    }

    public void ScoreIncrease(int ScoreAmount)
    {
        currentPlayerScore += ScoreAmount;
        ClientHandleScoreUpdated(0, currentPlayerScore);
    }

    /*public void TowerGenerateResource()
    {

    }*/

    public void ReduceResource(int resourceAmount)
    {
        int temp = currentPlayerResources;
        currentPlayerResources -= resourceAmount;
        ClientHandleResourcesUpdated(temp, currentPlayerResources);
    }

    public int GetResources()
    {
        return currentPlayerResources;
    }

    private void ClientHandleResourcesUpdated(int oldResources, int newResources)
    {
        ClientOnResourcesUpdated?.Invoke(newResources);
    }

    private void ClientHandlePlayerHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnPlayerHealthUpdated?.Invoke(newHealth);
    }

    private void ClientHandleScoreUpdated(int oldScore, int newScore)
    {
        ClientOnScoreUpdated?.Invoke(newScore);
    }

    #region Save and Load
        
    public object CaptureState()
    {
        return new SaveData
        {
            currentPlayerHealth = currentPlayerHealth,
            currentPlayerResources = currentPlayerResources,
            currentPlayerScore = currentPlayerScore
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        currentPlayerHealth = saveData.currentPlayerHealth;
        currentPlayerResources = saveData.currentPlayerResources;
        currentPlayerScore = saveData.currentPlayerScore;
        UpdateLoadProperties();
    }

    private void UpdateLoadProperties()
    {
        ClientHandlePlayerHealthUpdated(0, currentPlayerHealth);
        ClientHandleResourcesUpdated(0, currentPlayerResources);
        ClientHandleScoreUpdated(0, currentPlayerScore);
    }

    [Serializable]
    private struct SaveData
    {
        public int currentPlayerHealth;
        public int currentPlayerResources;
        public int currentPlayerScore;
    }
    
    #endregion

}
