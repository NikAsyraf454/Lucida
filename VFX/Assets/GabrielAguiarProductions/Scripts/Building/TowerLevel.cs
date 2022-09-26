using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerLevel : MonoBehaviour
{
    [SerializeField] public int towerId;
    [SerializeField] private string towerName;
    [SerializeField] private int maxTowerPrice;
    [SerializeField] private int currentTowerPrice;
    [SerializeField] private int purchasePriceIncrement;
    [SerializeField] private int _level;
    [SerializeField] private int _damageDeal;
    [SerializeField] private float _fireRate;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private bool isSpawned = false;        //for UI
    

    public int Level { get{return _level; } set{ _level = Level; } }
    public int DamageDeal { get{return _damageDeal; } set{ _damageDeal = DamageDeal; } }
    public float FireRate { get{return _fireRate; } set{ _fireRate = FireRate; } }

    // Start is called before the first frame update
    void Start()
    {
        //isSpawned = false;
        //currentTowerPrice = maxTowerPrice;
        _level = UnityEngine.Random.Range(1,4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public int GetDamageDeal()
    // {
    //     return damageDeal;
    // }

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

    // public int GetTowerLevel()
    // {
    //     return level;
    // }


    //TowerUpgrade()

    //Damage
}
