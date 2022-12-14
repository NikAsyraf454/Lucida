using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthDisplay : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth = null;
    [SerializeField] private GameObject healthBarParent = null;
    [SerializeField] private Image healthBarImage = null;

    private void Awake()
    {
        enemyHealth.ClientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnDisable()
    {
        enemyHealth.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    private void OnMouseEnter()
    {
        //healthBarParent.SetActive(true);
    }

    private void OnMouseExit()
    {
        //healthBarParent.SetActive(false);
    }

    private void HandleHealthUpdated(float currentHealth, float maxHealth)
    {
        healthBarImage.fillAmount = /* (float) */currentHealth / maxHealth;
    }
}

