using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerLevel : MonoBehaviour
{
    [SerializeField] public int towerId;
    [SerializeField] private string _towerName;
    [SerializeField] private int _maxTowerPrice;
    [SerializeField] private int _currentTowerPrice;
    [SerializeField] private int purchasePriceIncrement;
    [SerializeField] private int _level;
    [SerializeField] private int _damageDeal;
    [SerializeField] private float _fireRate;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private bool isSpawned = false;        //for UI
    

    public int Level { get{return _level; } set{ _level = Level; } }
    public int DamageDeal { get{return _damageDeal; } set{ _damageDeal = DamageDeal; } }
    public float FireRate { get{return _fireRate; } set{ _fireRate = FireRate; } }
    public string TowerName { get{return _towerName; } set{ _towerName = TowerName; } }
    public int MaxTowerPrice { get{return _maxTowerPrice; } set{ _maxTowerPrice = MaxTowerPrice; } }
    public int CurrentTowerPrice { get{return _currentTowerPrice; } set{ _currentTowerPrice = CurrentTowerPrice; } }
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
        return _towerName;
    }

    // public int GetCurrentTowerPrice()
    // {
    //     return currentTowerPrice;
    // }

    public bool GetAvailability()
    {
        _currentTowerPrice = _maxTowerPrice;      //set prefab price at start
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
        _currentTowerPrice += purchasePriceIncrement;
    }

    public void ResetTowerLevel()
    {
        SetSpawnedStatus(false);
        _currentTowerPrice = 0;
        //isUnlocked might change
    }

    // public int GetTowerLevel()
    // {
    //     return level;
    // }


    //TowerUpgrade()

    //Damage
}
