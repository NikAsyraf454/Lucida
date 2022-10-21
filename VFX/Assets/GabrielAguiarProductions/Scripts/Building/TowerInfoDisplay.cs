using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerInfoDisplay : MonoBehaviour
{
    private bool isDisplayed = false;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TowerLevel towerLevel;
    [SerializeField] private TMP_Text towerName, level, damageDeal, fireRate, sellPrice, cost;
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
        // Debug.Log("tower stats");
    }

    void OnMouseDown()
    {
        if(isDisplayed && !TowerManager.Instance.canDisplayInfo)
        {
            isDisplayed = false;
            statsPanel.SetActive(false);
            TowerManager.Instance.canDisplayInfo = true;
        }
        else if(TowerManager.Instance.canDisplayInfo)
        {
            statsPanel.SetActive(true);
            isDisplayed = true;
            TowerManager.Instance.canDisplayInfo = false;
        }
    }

    void UpdateStats(TowerLevel towerLevel)
    {
        damageDeal.SetText((towerLevel.DamageDeal * towerLevel.Level).ToString());
        fireRate.SetText(towerLevel.FireRate.ToString() + "/s");
        level.SetText("Level " + towerLevel.Level);
        xpBarImage.fillAmount = (float)towerLevel.TowerXp / (towerLevel.Level*10);
        cost.SetText("Cost: -$" + ((towerLevel.Level * 10) - towerLevel.TowerXp).ToString());
    }
}
