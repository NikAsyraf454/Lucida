using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerInfoDisplay : MonoBehaviour
{
    private bool isDisplayed;
    [SerializeField] private GameObject temp;
    [SerializeField] private TowerLevel towerLevel;
    [SerializeField] private TMP_Text towerName, level, damageDeal, fireRate, sellPrice;
    [SerializeField] private Image xpBarImage = null;

    private void Awake()
    {
        towerLevel.ServerOnTowerXp += UpdateStats;
    }

    private void OnDestroy()
    {
        towerLevel.ServerOnTowerXp -= UpdateStats;
    }

    void Start()
    {
        towerName.SetText(towerLevel.TowerName);
        sellPrice.SetText("Sell $" + towerLevel.MaxTowerPrice.ToString());
        UpdateStats(towerLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        Debug.Log("tower stats");
    }

    void OnMouseDown()
    {
        if(isDisplayed)
        {
            isDisplayed = false;
            temp.SetActive(false);
        }
        else
        {
            temp.SetActive(true);
            isDisplayed = true;
        }
    }

    void UpdateStats(TowerLevel towerLevel)
    {
        damageDeal.SetText((towerLevel.DamageDeal * towerLevel.Level).ToString());
        fireRate.SetText(towerLevel.FireRate.ToString() + "/s");
        level.SetText("Level " + towerLevel.Level);
        xpBarImage.fillAmount = (float)towerLevel.TowerXp / (towerLevel.Level*10);
    }
}
