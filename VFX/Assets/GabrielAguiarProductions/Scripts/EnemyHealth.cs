using System;
using System.Collections;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;

public class EnemyHealth : MonoBehaviour//NetworkBehaviour
{
    public PlayerManager playerManager;
    [SerializeField] private int maxHealth = 100;

   //[SyncVar(hook = nameof(HandleHealthUpdated))]
    [SerializeField] private int _currentHealth;

    [SerializeField] private int resourceDrop;
    [SerializeField] private int scoreValue;


    public event Action<GameObject> ServerOnDie;

    public event Action<int, int> ClientOnHealthUpdated;

    public int CurrentHealth { get{return _currentHealth; } set{ _currentHealth = CurrentHealth; } }

    private void Start()
    {
        _currentHealth = maxHealth;

    }

    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }
    
    public void DealDamage(int damageAmount)
    {
        if (_currentHealth == 0) { return; }

        _currentHealth = Mathf.Max(_currentHealth - damageAmount, 0);
        HandleHealthUpdated(maxHealth, _currentHealth);

        if (_currentHealth != 0) { return; }

        //death of enemy
        EnemyDeath();
        playerManager.MobDropResource(resourceDrop);
        playerManager.ScoreIncrease(scoreValue);
        //ServerOnDie?.Invoke();
    }

    public void EnemyDeath()
    {

        ServerOnDie?.Invoke(gameObject);
        Destroy(this.gameObject);  
    }

/*
    #region Server

    public override void OnStartServer()
    {
        currentHealth = maxHealth;

        UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnPlayerDie -= ServerHandlePlayerDie;
    }

    //[Server]
    private void ServerHandlePlayerDie(int connectionId)
    {
        if (connectionToClient.connectionId != connectionId) { return; }

        DealDamage(currentHealth);
    }

    //[Server]
    public void DealDamage(int damageAmount)
    {
        if (currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth != 0) { return; }

        ServerOnDie?.Invoke();
    }

    #endregion

    #region Client

    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }

    #endregion
    */
}

