using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevel : MonoBehaviour
{
    [SerializeField] private string towerName;
    [SerializeField] private int maxTowerPrice;
    [SerializeField] private int currentTowerPrice;
    [SerializeField] private int purchasePriceIncrement;
    [SerializeField] private int level;
    [SerializeField] private int damageDeal;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private bool isSpawned = false;        //for UI
    [SerializeField] public int towerId;
    

    // Start is called before the first frame update
    void Start()
    {
        //isSpawned = false;
        //currentTowerPrice = maxTowerPrice;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetDamageDeal()
    {
        return damageDeal;
    }

    public string GetTowerName()
    {
        return towerName;
    }

    public int GetCurrentTowerPrice()
    {
        return currentTowerPrice;
    }

    public bool GetAvailability()
    {
        currentTowerPrice = maxTowerPrice;      //set prefab price at start
        return isUnlocked;
    }

    public bool GetSpawnedStatus()
    {
        return isSpawned;
    }

    public void SetSpawnedStatus(bool status)
    {
        isSpawned = status;
    }

    public void SetNewTowerPrice()
    {
        currentTowerPrice += purchasePriceIncrement;
    }

    public void ResetTowerLevel()
    {
        SetSpawnedStatus(false);
        currentTowerPrice = 0;
        //isUnlocked might change
    }

    //TowerUpgrade()

    //Damage
}
