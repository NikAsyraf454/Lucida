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

    public int currentCharge = 0;
    [SerializeField] private int maxCharge = 3;
    //public int chargeCapacity;

    [SerializeField] private int resourceMultiplier = 1;

    public event Action<int> ClientOnResourcesUpdated;
    public event Action<int> ClientOnPlayerHealthUpdated;
    public event Action<int> ClientOnScoreUpdated;
    public event Action<int> ClientOnChargeUpdated;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        ClientHandlePlayerHealthUpdated(0, currentPlayerHealth);
        currentPlayerResources = maxPlayerResources;
        ClientHandleResourcesUpdated(0, currentPlayerResources);
        currentPlayerScore = maxPlayerScore;
        ClientHandleScoreUpdated(0, currentPlayerScore);
        currentCharge = maxCharge;
        ClientHandleChargeUpdated(0, currentCharge);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReducePlayerHealth(int damageAmount)
    {
        CameraShake.Shake(0.25f, 0.2f);
        int temp = currentPlayerHealth;
        currentPlayerHealth -= damageAmount;
        ClientHandlePlayerHealthUpdated(temp, currentPlayerHealth);

        // if(currentPlayerHealth <= 5) { SecondChance(); } //second chance for player (eliminate all enemy in wave or map)
        if(currentPlayerHealth <= 0) 
        {
            currentPlayerHealth = 0;
            MenuManager.Instance.PlayerLose();
        }
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

    public void IncreaseCharge(int chargeAmount)
    {
        if((currentCharge + chargeAmount) > maxCharge)
        {
            currentCharge = maxCharge;
        }
        else
        {
            currentCharge += chargeAmount;
        }
        
        ClientHandleChargeUpdated(0, currentCharge);
    }

    public bool ReduceCharge(int chargeAmount)
    {
        if((currentCharge - chargeAmount) < 0) { return false; }
        currentCharge -= chargeAmount;
        ClientHandleChargeUpdated(0, currentCharge);
        return true;
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

    private void ClientHandleChargeUpdated(int oldCharge, int newCharge)
    {
        ClientOnChargeUpdated?.Invoke(newCharge);
    }

    #region Save and Load
        
    public object CaptureState()
    {
        return new SaveData
        {
            currentPlayerHealth = currentPlayerHealth,
            currentPlayerResources = currentPlayerResources,
            currentPlayerScore = currentPlayerScore,
            currentCharge = currentCharge
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        currentPlayerHealth = saveData.currentPlayerHealth;
        currentPlayerResources = saveData.currentPlayerResources;
        currentPlayerScore = saveData.currentPlayerScore;
        currentCharge = saveData.currentCharge;
        UpdateLoadProperties();
    }

    private void UpdateLoadProperties()
    {
        ClientHandlePlayerHealthUpdated(0, currentPlayerHealth);
        ClientHandleResourcesUpdated(0, currentPlayerResources);
        ClientHandleScoreUpdated(0, currentPlayerScore);
        ClientHandleChargeUpdated(0, currentCharge);
    }

    [Serializable]
    private struct SaveData
    {
        public int currentPlayerHealth;
        public int currentPlayerResources;
        public int currentPlayerScore;
        public int currentCharge;
    }
    
    #endregion

}
