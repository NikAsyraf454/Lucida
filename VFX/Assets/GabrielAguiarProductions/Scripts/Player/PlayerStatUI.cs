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
    [SerializeField] private GameObject[] healthBar;

    private void Awake()
    {
        playerManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();
        playerManager.ClientOnPlayerHealthUpdated += ClientHandlePlayerHealthUpdated;
        playerManager.ClientOnResourcesUpdated += ClientHandleResourcesUpdated;
        playerManager.ClientOnScoreUpdated += ClientHandleScoreUpdated;
        
    }

    private void OnDestroy()
    {
        playerManager.ClientOnResourcesUpdated -= ClientHandleResourcesUpdated;
        playerManager.ClientOnPlayerHealthUpdated -= ClientHandlePlayerHealthUpdated;
        playerManager.ClientOnScoreUpdated -= ClientHandleScoreUpdated;
    }

    // Start is called before the first frame update


    // Update is called once per frame
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

}
