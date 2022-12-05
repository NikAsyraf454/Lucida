using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerManager : MonoBehaviour, ISaveable
{
    public static TowerManager Instance;
    public TowerButton towerButton;
    public List<GameObject> towerPrefabList;

    public List<TowerLevel> towerLevelList;

    public List<TowerLevel> spawnedTowerList;

    public GameObject towerInstance = null;
    private int towerInstanceId;
    public bool canDisplayInfo = true;
    // [SerializeField] private PlayerManager playerManager;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        // playerManager = GetComponent<PlayerManager>();
        CacheTowerLevels();
        towerButton = GameObject.FindGameObjectWithTag("TowerShop").GetComponent<TowerButton>();
        CheckTowerAvailability();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused || Input.GetMouseButtonDown(1))
        {
            towerInstance = null;
            //close ui of tower info
        }
    }

    private void OnDestroy()
    {
        foreach(TowerLevel towerLevel in towerLevelList)
        {
                towerLevel.ResetTowerLevel();
                towerLevel.ServerOnTowerDestroyed -= RemoveTower;
                towerLevel.ServerOnUpgradeTowerLevel -= ImproveTower;
                spawnedTowerList.Remove(towerLevel);
        }
    }

    public void SetTowerInstance(int towerId)
    {
        towerInstance = towerPrefabList[towerId];
        towerInstanceId = towerId;
    }

    public bool BuildTower(Vector3 position)
    {
        if (PlayerManager.Instance.GetResources() < towerLevelList[towerInstanceId].CurrentTowerPrice) { return true; }
        
        
        PlayerManager.Instance.ReduceResource(towerLevelList[towerInstanceId].CurrentTowerPrice);
        
        SpawnTower(position);
        return false;
    }

    private void SpawnTower(Vector3 position)
    {
        if(!GetNodeAvailability(position)) { return; }      //raycast, check if node below the position can be build
        position.y = towerLevelList[towerInstanceId].SpawnHeight;

        GameObject tower = Instantiate(towerInstance, position, Quaternion.identity);
        spawnedTowerList.Add(tower.GetComponentInChildren<TowerLevel>());
        spawnedTowerList[spawnedTowerList.Count-1].ServerOnTowerDestroyed += RemoveTower;
        spawnedTowerList[spawnedTowerList.Count-1].ServerOnUpgradeTowerLevel += ImproveTower;
        towerLevelList[towerInstanceId].SetNewTowerPrice();
        towerButton.UpdateShopItemText(towerInstanceId);
    }

    public void RemoveTower(TowerLevel towerLevel)
    {
        PlayerManager.Instance.IncreaseResource(towerLevel.MaxTowerPrice);
        spawnedTowerList.Remove(towerLevel);
        foreach(TowerLevel towerLevel1 in spawnedTowerList)
        {
            if(towerLevel1.gameObject == towerLevel.gameObject)
                towerLevel1.ServerOnTowerDestroyed -= RemoveTower;
                // towerLevel1.ServerOnUpgradeTowerLevel -= ImproveTower;
        }
    }

    public void ImproveTower(TowerLevel towerLevel)
    {
        int cost = (towerLevel.Level * 10) - towerLevel.TowerXp;
        if(PlayerManager.Instance.GetResources() < cost) {return;}
        PlayerManager.Instance.ReduceResource(cost);
        towerLevel.XpIncrease(cost);

    }


    public void CacheTowerLevels()
    {
        foreach(GameObject tower in towerPrefabList)
        {
            towerLevelList.Add(tower.GetComponentInChildren<TowerLevel>());
        }
    }

    public void CheckTowerAvailability()        //available in trems of unlocked
    {
        foreach(TowerLevel towerLevel in towerLevelList)
        {
            if(towerLevel.GetAvailability() && !towerLevel.GetSpawnedStatus())
            {
                towerButton.SpawnShopItem(towerLevel);
                towerLevel.SetSpawnedStatus(true);
                //towerButton.SetShopItemPriceText(towerLevel);
            }
        }
    }

    private bool GetNodeAvailability(Vector3 position)
    {
        Ray ray = new Ray(position, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            
            return hit.collider.gameObject.GetComponentInChildren<Node>().canBuild;
        }
        return false;
    }


    #region Save and Load

    public object CaptureState()
    {
        //temp = new List<int>();
        //tempPosition = new List<float[]>();
        //tempPosition = new List<float[3]>();
        List<int> temp = new List<int>();
        List<float> tempPosition = new List<float>();
        List<int> levels = new List<int>();
        List<int> damageDeal = new List<int>();
        List<int> exps = new List<int>();
        foreach( TowerLevel towerLevel in spawnedTowerList)
        {
            temp.Add(towerLevel.towerId);
            tempPosition.Add(towerLevel.gameObject.transform.position.x);
            tempPosition.Add(towerLevel.gameObject.transform.position.y);
            tempPosition.Add(towerLevel.gameObject.transform.position.z);
            levels.Add(towerLevel.Level);
            damageDeal.Add(towerLevel.DamageDeal);
            exps.Add(towerLevel.TowerXp);

        }
        return new SaveData
        {
            towerIdList = temp,
            towerPosition = tempPosition,
            level = levels,
            damageDeal = damageDeal,
            exps = exps
        };
    }

    public void RestoreState(object state)
    {
        
        var saveData = (SaveData)state;
        UpdateLoadProperties(saveData);
    }

    private void UpdateLoadProperties(SaveData saveData)         //if any properties needed to be updated for UI or etc
    {
        List<int> towerIdList = new List<int>();
        List<float> towerPosition = new List<float>();
        List<int> levels = new List<int>();
        List<int> damageDeal = new List<int>();
        List<int> exps = new List<int>();

        towerIdList = saveData.towerIdList;
        towerPosition = saveData.towerPosition;
        levels = saveData.level;
        damageDeal = saveData.damageDeal;
        exps = saveData.exps;
        int j = 0;
        foreach(int i in towerIdList)
        {
            SetTowerInstance(i);
            SpawnTower(new Vector3(towerPosition[(j*3)],towerPosition[(j*3)+1],towerPosition[(j*3)+2]));
            spawnedTowerList[j].Level = levels[j];
            spawnedTowerList[j].DamageDeal = damageDeal[j];
            spawnedTowerList[j].TowerXp = exps[j];
            j++;
        }

    }

    [Serializable]
    private struct SaveData
    {
        public List<int> towerIdList;
        public List<float> towerPosition;
        public List<int> level;
        public List<int> damageDeal; 
        public List<int> exps;
        //public List<float> towerTimer;
    }

    #endregion
}
