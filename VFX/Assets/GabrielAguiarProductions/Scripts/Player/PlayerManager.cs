using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveable
{
    public static PlayerManager Instance;
    public int currentPlayerHealth = 0;
    [SerializeField] private int maxPlayerHealth;

    public int currentPlayerResources = 0;
    [SerializeField] private int maxPlayerResources;

    public int currentPlayerScore = 0;
    [SerializeField] private int maxPlayerScore;

    [SerializeField] private int resourceMultiplier = 1;

    public event Action<int> ClientOnResourcesUpdated;
    public event Action<int> ClientOnPlayerHealthUpdated;
    public event Action<int> ClientOnScoreUpdated;




    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
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

    public void ReducePlayerHealth(int damageAmount)
    {
        CameraShake.Shake(0.25f, 4f);
        int temp = currentPlayerHealth;
        currentPlayerHealth -= damageAmount;
        ClientHandlePlayerHealthUpdated(temp, currentPlayerHealth);

        // if(currentPlayerHealth <= 5) { SecondChance(); } //second chance for player (eliminate all enemy in wave or map)
        if(currentPlayerHealth <= 0) { MenuManager.Instance.PlayerLose(); }
    }

    public void IncreaseResource(int resourceAmount)
    {
        // int temp = currentPlayerResources;
        currentPlayerResources += resourceAmount * resourceMultiplier;
        ClientHandleResourcesUpdated(0, currentPlayerResources);

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

    public int GetFinalScore()
    {
        return currentPlayerScore * resourceMultiplier;
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
