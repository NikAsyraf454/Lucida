using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private int originalDamage;
    [SerializeField] private float _fireRate;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private bool isSpawned = false;        //for UI]
    [SerializeField] private int _towerXp;
    [SerializeField] private float _towerRange;
    [SerializeField] private GameObject _powerUpVFX;
    [SerializeField] private float _spawnHeight;
    private SphereCollider sphereCollider;

    public Node node;
    public string inputKey;
    public Image icon;
    
    

    public int Level { get{return _level; } set{ _level = Level; } }
    public int DamageDeal { get{return _damageDeal; } set{ _damageDeal = DamageDeal; } }
    public float FireRate { get{return _fireRate; } set{ _fireRate = FireRate; } }
    public string TowerName { get{return _towerName; } set{ _towerName = TowerName; } }
    public int MaxTowerPrice { get{return _maxTowerPrice; } set{ _maxTowerPrice = MaxTowerPrice; } }
    public int CurrentTowerPrice { get{return _currentTowerPrice; } set{ _currentTowerPrice = CurrentTowerPrice; } }
    public int TowerXp { get{return _towerXp; } set{ _towerXp = TowerXp; } }
    public float TowerRange { get{return _towerRange; } set{ _towerRange = TowerRange; } }
    public float SpawnHeight { get{return _spawnHeight; } set{ _spawnHeight = SpawnHeight; } }

    public event Action<TowerLevel> ServerOnTowerXp;
    public event Action<TowerLevel> ServerOnTowerDestroyed;
    public event Action<TowerLevel> ServerOnUpgradeTowerLevel;


    // Start is called before the first frame update
    void Start()
    {
        //isSpawned = false;
        //currentTowerPrice = maxTowerPrice;
        // _level = UnityEngine.Random.Range(1,4);
        GetNode(transform.position);
        originalDamage = _damageDeal;
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = _towerRange;
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
        _level = 1;
        _towerXp = 0;
        //isUnlocked might change
    }

    public void XpIncrease(int increaseAmount)
    {
        _towerXp += increaseAmount;
        // Debug.Log(TowerXp + " " + increaseAmount);

        if(TowerXp >= Level*10) 
        {
            _level++; 
            _towerXp = 0; 
            _damageDeal = originalDamage * _level;
        }

        

        ServerOnTowerXp.Invoke(this);
    }

    public void IncreaseTowerLevel()
    {
        // int cost = (_level * 10) - _towerXp;
        ServerOnUpgradeTowerLevel.Invoke(this);
    }

    public void SellTower()
    {
        RotateToEnemyScript rotateToEnemyScript = GetComponent<RotateToEnemyScript>();
        if(rotateToEnemyScript != null)
            rotateToEnemyScript.fireDelay(5f);
        ServerOnTowerDestroyed.Invoke(this);
        node.TowerDestroyed();
        Destroy(transform.parent.gameObject, 0.2f);
    }

    public void UpdateColliderRadius()
    {
        sphereCollider.radius = _towerRange;
    }

    // public int GetTowerLevel()
    // {
    //     return level;
    // }


    //TowerUpgrade()

    //Damage

    private void GetNode(Vector3 position)
    {
        position.y = 0.35f;
        Ray ray = new Ray(position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.DrawRay(position, Vector3.down, Color.green);
            node =  hit.collider.gameObject.GetComponentInChildren<Node>();
            node.BuildTower();
        }
        if(node == null)
        {
            Debug.Log("Node not found");
        }
    }

    public void DoIncreaseDamage(float increase, float duration)
    {
        StartCoroutine(IncreaseDamage(increase, duration));
    }

    //increase takes in value of 0% - 100%
    IEnumerator IncreaseDamage(float increase, float duration)
    {
        int temp = _damageDeal;
        float calcu = _damageDeal*(1+(increase/100));
        // Debug.Log(temp + " damage increased to " + (_damageDeal*(1+(increase/100))));
        _damageDeal = (int)calcu;
        ServerOnTowerXp.Invoke(this);
        _powerUpVFX.SetActive(true);
        yield return new WaitForSeconds (duration);
        _damageDeal = temp;
        ServerOnTowerXp.Invoke(this);
        _powerUpVFX.SetActive(false);
        _powerUpVFX.SetActive(false);
        StopCoroutine("IncreaseDamage");
    }
}
