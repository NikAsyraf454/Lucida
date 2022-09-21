using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private GameObject shopItem;
    [SerializeField] private List<GameObject> shopItemList;
    [SerializeField] private List<TowerLevel> towerLevelList; 
    //[SerializeField] private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        towerManager = FindObjectOfType<TowerManager>();
        //playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        //call tower manager to run throught every tower prefab
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        foreach(GameObject shopitem in shopItemList)
            shopItemList.Remove(shopItem);

        foreach(TowerLevel towerLevel in towerLevelList)
            towerLevelList.Remove(towerLevel);
        
    }

    public void PurchaseTower(int towerId)      //function for UI button
    {
        towerManager.SetTowerInstance(towerId);
    }

    public void SpawnShopItem(TowerLevel towerLevel)
    {
        GameObject shopItemInstance = Instantiate(shopItem, transform.position, Quaternion.identity);
        shopItemList.Add(shopItemInstance);
        towerLevelList.Add(towerLevel);
        SetShopItemProperties(shopItemInstance, towerLevel);
    }

    public void SetShopItemProperties(GameObject shopItemInstance, TowerLevel towerLevel)
    {
        //shopItemInstance.transform.parent = gameObject.transform;
        shopItemInstance.transform.SetParent(gameObject.transform, false);
        shopItemInstance.GetComponent<Button>().onClick.AddListener(delegate{PurchaseTower(towerLevel.towerId);});
        SetShopItemText(shopItemInstance, towerLevel);

    }

    public void SetShopItemText(GameObject shopItem, TowerLevel towerLevel)
    {
        TMP_Text[] text = shopItem.GetComponentsInChildren<TMP_Text>();
        text[0].text = towerLevel.GetTowerName();
        text[1].text = $" ${towerLevel.GetCurrentTowerPrice().ToString()}";
    }

    public void UpdateShopItemText(int towerLevelId)
    {
        for(int i = 0; i < towerLevelList.Count; i++)
        {
            
            if(towerLevelList[i].towerId != towerLevelId) {continue;}

            TMP_Text[] text = shopItemList[i].GetComponentsInChildren<TMP_Text>();
            text[0].text = towerLevelList[i].GetTowerName();
            text[1].text = $" ${towerLevelList[i].GetCurrentTowerPrice().ToString()}";
        }
    }
}
