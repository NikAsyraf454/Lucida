using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public TowerButton towerButton;
    public List<GameObject> towerPrefabList;

    public List<TowerLevel> towerLevelList;

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
        }
    }

    public void SetTowerInstance(int towerId)
    {
        towerInstance = towerPrefabList[towerId];
        towerInstanceId = towerId;
    }

    public bool BuildTower(Vector3 position)
    {
        if (playerManager.GetResources() < towerLevelList[towerInstanceId].GetCurrentTowerPrice()) { return true; }
        playerManager.BuildTowerResource(towerLevelList[towerInstanceId].GetCurrentTowerPrice());
        Instantiate(towerInstance, position, Quaternion.identity);
        towerLevelList[towerInstanceId].SetNewTowerPrice();
        towerButton.UpdateShopItemText(towerInstanceId);
        return false;
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
            towerLevelList.Add(tower.GetComponent<TowerLevel>());
        }
    }

    public void CheckTowerAvailability()
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
}
