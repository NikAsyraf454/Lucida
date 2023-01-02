using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveable
{
    public static PlayerManager Instance;
    public int currentPlayerHealth = 0;
    public int maxPlayerHealth;

    public float currentPlayerResources = 0;
    [SerializeField] private int maxPlayerResources;

    public int currentPlayerScore = 0;
    [SerializeField] private int maxPlayerScore;

    public int currentCharge = 0;
    public int maxCharge = 5;
    //public int chargeCapacity;

    private DamageScreen damageScreen;

    private Lifeline lifeline;
    [SerializeField] private bool gotLifeline = false;

    [SerializeField] private float resourceMultiplier = 1f;
    [SerializeField] private int scoreMultiplier = 1;

    public event Action<int> ClientOnResourcesUpdated;
    public event Action<int> ClientOnPlayerHealthUpdated;
    public event Action<int> ClientOnScoreUpdated;
    public event Action<int> ClientOnChargeUpdated;

    public enum Difficulty{
        Easy,
        Normal,
        Hard
    };

    public Difficulty difficulty;

    void Awake()
    {
        Instance = this;  
    }

    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
        ClientHandlePlayerHealthUpdated(0, currentPlayerHealth);
        currentPlayerResources = maxPlayerResources;
        ClientHandleResourcesUpdated(0, (int)currentPlayerResources);
        currentPlayerScore = maxPlayerScore;
        ClientHandleScoreUpdated(0, currentPlayerScore);
        currentCharge = 0;
        ClientHandleChargeUpdated(0, currentCharge);
        lifeline = GetComponent<Lifeline>();
        damageScreen = GetComponent<DamageScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReducePlayerHealth(int damageAmount)
    {
        currentPlayerHealth -= damageAmount;

        if(currentPlayerHealth <= 0) 
        {
            currentPlayerHealth = 0;
            MenuManager.Instance.PlayerLose();
        }

        CameraShake.Shake(0.25f, 0.2f);
        // int temp = currentPlayerHealth;
        ClientHandlePlayerHealthUpdated(0, currentPlayerHealth);
        damageScreen.PlayerTakeDamage();

        if(currentPlayerHealth <= 3)    //second chance for player (eliminate all enemy in wave or map)
        {
            gotLifeline = true;
            if(!gotLifeline)
            {
                int temp = lifeline.GaveLifeline();
                // Debug.Log("Temp: " + temp);
                currentPlayerHealth += temp;
                ClientHandlePlayerHealthUpdated(0, currentPlayerHealth);
            }
        } 


    }

    public void IncreaseResource(int resourceAmount)
    {
        // int temp = currentPlayerResources;
        currentPlayerResources += (resourceAmount * resourceMultiplier);
        ClientHandleResourcesUpdated(0, (int)currentPlayerResources);
        TowerButton.Instance.UpdateButtonInteractable();

    }

    public void ScoreIncrease(int ScoreAmount)
    {
        currentPlayerScore += (ScoreAmount * scoreMultiplier);
        ClientHandleScoreUpdated(0, currentPlayerScore);
    }

    /*public void TowerGenerateResource()
    {

    }*/

    public void ReduceResource(int resourceAmount)
    {
        currentPlayerResources -= resourceAmount;
        ClientHandleResourcesUpdated(0, (int)currentPlayerResources);
    }

    public int GetResources()
    {
        return (int)currentPlayerResources;
    }

    public int GetFinalScore()
    {
        return currentPlayerScore/*  * resourceMultiplier */;
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

    public bool CheckCharge(int chargeAmount)
    {
        if((currentCharge - chargeAmount) < 0) { return false; }

        return true;
    }

    public void ReduceCharge(int chargeAmount)
    {
        currentCharge -= chargeAmount;
        ClientHandleChargeUpdated(0, currentCharge);
    }

    public void DoIncreaseMultiplier(float increase, float duration)
    {
        StartCoroutine(IncreaseMultiplier(increase, duration));
    }

    IEnumerator IncreaseMultiplier(float increase, float duration)
    {
        float temp = resourceMultiplier;
        float calcu = resourceMultiplier*(1+(increase/100));
        resourceMultiplier = calcu;
        yield return new WaitForSeconds (duration);
        resourceMultiplier = temp;
        StopCoroutine("IncreaseMultiplier");
    }

    public void SetDifficulty(Difficulty i)
    {
        (difficulty) = i;
        switch(difficulty)
        {
            case Difficulty.Easy:
                scoreMultiplier = 1;
                break;
            case Difficulty.Normal:
                scoreMultiplier = 2;
                break;
            case Difficulty.Hard:
                scoreMultiplier = 3;
                break;
            
        }
        WaveManager.Instance.PopulateWavesList();
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
            currentPlayerResources = (int)currentPlayerResources,
            currentPlayerScore = currentPlayerScore,
            currentCharge = currentCharge,
            gotLifeline = gotLifeline,
            difficulty = difficulty
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        currentPlayerHealth = saveData.currentPlayerHealth;
        currentPlayerResources = saveData.currentPlayerResources;
        currentPlayerScore = saveData.currentPlayerScore;
        currentCharge = saveData.currentCharge;
        gotLifeline = saveData.gotLifeline;
        difficulty = saveData.difficulty;
        UpdateLoadProperties();
    }

    private void UpdateLoadProperties()
    {
        ClientHandlePlayerHealthUpdated(0, currentPlayerHealth);
        ClientHandleResourcesUpdated(0, (int)currentPlayerResources);
        ClientHandleScoreUpdated(0, currentPlayerScore);
        ClientHandleChargeUpdated(0, currentCharge);
        SetDifficulty(difficulty);
    }

    [Serializable]
    private struct SaveData
    {
        public int currentPlayerHealth;
        public int currentPlayerResources;
        public int currentPlayerScore;
        public int currentCharge;
        public bool gotLifeline;
        public Difficulty difficulty;
    }
    
    #endregion

}
