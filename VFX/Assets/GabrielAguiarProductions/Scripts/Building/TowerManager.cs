using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerManager : MonoBehaviour, ISaveable
{
    public TowerButton towerButton;
    public List<GameObject> towerPrefabList;

    public List<TowerLevel> towerLevelList;

    public List<TowerLevel> spawnedTowerList;

    public GameObject towerInstance = null;
    private int towerInstanceId;
    [SerializeField] private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
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
        if (playerManager.GetResources() < towerLevelList[towerInstanceId].CurrentTowerPrice) { return true; }

        playerManager.ReduceResource(towerLevelList[towerInstanceId].CurrentTowerPrice);
        SpawnTower(position);
        return false;
    }

    private void SpawnTower(Vector3 position)
    {
        GameObject tower = Instantiate(towerInstance, position, Quaternion.identity);
        spawnedTowerList.Add(tower.GetComponentInChildren<TowerLevel>());
        spawnedTowerList[spawnedTowerList.Count-1].ServerOnTowerDestroyed += RemoveTower;
        towerLevelList[towerInstanceId].SetNewTowerPrice();
        towerButton.UpdateShopItemText(towerInstanceId);
    }

    public void RemoveTower(TowerLevel towerLevel)
    {
        playerManager.IncreaseResource(towerLevel.MaxTowerPrice);
        spawnedTowerList.Remove(towerLevel);
        foreach(TowerLevel towerLevel1 in spawnedTowerList)
        {
            if(towerLevel1.gameObject == towerLevel.gameObject)
                towerLevel1.ServerOnTowerDestroyed -= RemoveTower;
        }
        // Destroy(towerLevel.gameObject);
    }


/*     public int GetRegularTowerPrice()
    {
        return towerLevelList[0].GetCurrentTowerPrice();
    }

    public int GetMortarTowerPrice()
    {
        return towerLevelList[1].GetCurrentTowerPrice();
    } */

    //public void NullTowerInstance

    //public void checkTowerAvailability()

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
        foreach( TowerLevel towerLevel in spawnedTowerList)
        {
            temp.Add(towerLevel.towerId);
            tempPosition.Add(towerLevel.gameObject.transform.position.x);
            tempPosition.Add(towerLevel.gameObject.transform.position.y);
            tempPosition.Add(towerLevel.gameObject.transform.position.z);
            levels.Add(towerLevel.Level);
            damageDeal.Add(towerLevel.DamageDeal);

        }
        return new SaveData
        {
            towerIdList = temp,
            towerPosition = tempPosition,
            level = levels,
            damageDeal = damageDeal
        };
    }

    public void RestoreState(object state)
    {
        List<int> temp = new List<int>();
        List<float> tempPosition = new List<float>();
        List<int> levels = new List<int>();
        List<int> damageDeal = new List<int>();
        var saveData = (SaveData)state;

        temp = saveData.towerIdList;
        tempPosition = saveData.towerPosition;
        levels = saveData.level;
        damageDeal = saveData.damageDeal;
        
        UpdateLoadProperties(temp, tempPosition, levels, damageDeal);
    }

    private void UpdateLoadProperties(List<int> towerIdList, List<float> towerPosition, List<int> levels, List<int> damageDeal)         //if any properties needed to be updated for UI or etc
    {

        int j = 0;
        foreach(int i in towerIdList)
        {
            SetTowerInstance(i);
            SpawnTower(new Vector3(towerPosition[j*3],towerPosition[j*3+1],towerPosition[j*3+2]));
            spawnedTowerList[j].Level = levels[j];
            spawnedTowerList[j].DamageDeal = damageDeal[j];
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
        //public List<float> towerTimer;
    }

    #endregion
}
