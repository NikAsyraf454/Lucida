using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerInfoDisplay : MonoBehaviour
{
    private bool isDisplayed;
    [SerializeField] private GameObject temp;
    [SerializeField] private TowerLevel towerLevel;
    [SerializeField] private TMP_Text towerName, level, damageDeal, fireRate, sellPrice;

    void Start()
    {
        towerName.SetText(towerLevel.TowerName);
        sellPrice.SetText(towerLevel.MaxTowerPrice.ToString());
        damageDeal.SetText(towerLevel.DamageDeal.ToString());
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
}
