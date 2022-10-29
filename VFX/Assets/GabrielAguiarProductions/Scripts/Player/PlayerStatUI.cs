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
    [SerializeField] private GameObject[] healthBar;

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
        for(int i = 0; i < health; i++)
        {
            healthBar[i].gameObject.SetActive(true);
        }
        for(int i = health; i < 20; i++)
        {
            healthBar[i].gameObject.SetActive(false);
        }
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
        //resourcesText.text = $"Score: {score}";
        chargeText.text = $"{charge}";
    }

}
