using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerInfoDisplay : MonoBehaviour
{
    private bool isDisplayed = false;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject towerRange;
    [SerializeField] private TowerLevel towerLevel;
    [SerializeField] private TMP_Text towerName, level, damageDeal, fireRate, sellPrice, cost;
    [SerializeField] private Image xpBarImage = null;
    private Torus torusScript;

    private void Awake()
    {
        towerLevel.ServerOnTowerXp += UpdateStats;
        towerLevel.ServerOnTowerDestroyed += TowerDestroyed;
    }

    private void OnDestroy()
    {
        towerLevel.ServerOnTowerXp -= UpdateStats;
        towerLevel.ServerOnTowerDestroyed -= TowerDestroyed;
    }

    void Start()
    {
        towerName.SetText(towerLevel.TowerName);
        sellPrice.SetText("Sell $" + towerLevel.MaxTowerPrice.ToString());
        UpdateStats(towerLevel);
        torusScript = towerRange.GetComponent<Torus>();
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
        DisplayTowerInfo();
    }
    
    public void DisplayTowerInfo()
    {
        if(isDisplayed && !TowerManager.Instance.canDisplayInfo)
        {
            isDisplayed = false;
            statsPanel.SetActive(false);
            towerRange.SetActive(false);
            TowerManager.Instance.canDisplayInfo = true;
        }
        else if(TowerManager.Instance.canDisplayInfo)
        {
            statsPanel.SetActive(true);
            towerRange.SetActive(true);
            torusScript.radius = (towerLevel.TowerRange/2);
            torusScript.NewMesh();
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

    void TowerDestroyed(TowerLevel towerLevel)
    {
        if(isDisplayed)
        TowerManager.Instance.canDisplayInfo = true;
    }
}
