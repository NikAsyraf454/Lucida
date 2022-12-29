using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatUI : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private TMP_Text healthText = null;
    [SerializeField] private TMP_Text resourcesText = null;
    [SerializeField] private TMP_Text scoreText = null;
    [SerializeField] private TMP_Text chargeText = null;
    // [SerializeField] private GameObject[] healthBar;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject[] chargeBar;

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        playerManager.ClientOnPlayerHealthUpdated += ClientHandlePlayerHealthUpdated;
        playerManager.ClientOnResourcesUpdated += ClientHandleResourcesUpdated;
        playerManager.ClientOnScoreUpdated += ClientHandleScoreUpdated;
        playerManager.ClientOnChargeUpdated += ClientHandleChargeUpdated;
        
    }

    private void OnDestroy()
    {
        playerManager.ClientOnResourcesUpdated -= ClientHandleResourcesUpdated;
        playerManager.ClientOnPlayerHealthUpdated -= ClientHandlePlayerHealthUpdated;
        playerManager.ClientOnScoreUpdated -= ClientHandleScoreUpdated;
        playerManager.ClientOnChargeUpdated -= ClientHandleChargeUpdated;
    }

    void Update()
    {
        
    }

    private void ClientHandlePlayerHealthUpdated(int health)
    {
        //resourcesText.text = $"Health: {health}";
        healthText.text = $"{health}";
        Debug.Log((float)(health / PlayerManager.Instance.maxPlayerHealth) + " = " + health + " / " + PlayerManager.Instance.maxPlayerHealth);
        float temp = (float)(health / PlayerManager.Instance.maxPlayerHealth);
        healthBar.fillAmount = temp;
        // for(int i = 0; i < health; i++)
        // {
        //     healthBar[i].gameObject.SetActive(true);
        // }
        // for(int i = health; i < 20; i++)
        // {
        //     healthBar[i].gameObject.SetActive(false);
        // }
    }

    private void ClientHandleResourcesUpdated(int resources)
    {
        //resourcesText.text = $"Coin: {resources}";
        resourcesText.text = $"{resources}";
    }

    private void ClientHandleScoreUpdated(int score)
    {
        //resourcesText.text = $"Score: {score}";
        scoreText.text = $"{score}";
    }

    private void ClientHandleChargeUpdated(int charge)
    {
        chargeText.text = $"Charge: {charge}";
                for(int i = 0; i < charge; i++)
        {
            chargeBar[i].gameObject.SetActive(true);
        }
        for(int i = charge; i < PlayerManager.Instance.maxCharge; i++)
        {
            chargeBar[i].gameObject.SetActive(false);
        }
    }

}
