using System;
using System.Collections;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;

public class EnemyHealth : MonoBehaviour//NetworkBehaviour
{
    public PlayerManager playerManager;
    [SerializeField] private float maxHealth = 100f;

   //[SyncVar(hook = nameof(HandleHealthUpdated))]
    [SerializeField] private float _currentHealth;

    [SerializeField] private int resourceDrop;
    [SerializeField] private int scoreValue;

    [SerializeField] private Renderer _renderer;
    private Material _material;

    public event Action<GameObject> ServerOnDie;

    public event Action<float, float> ClientOnHealthUpdated;

    public float CurrentHealth { get{return _currentHealth; } set{ _currentHealth = CurrentHealth; } }

    private void Start()
    {
        _currentHealth = maxHealth;

        if(_renderer == null)
        {
            _material = GetComponent<Renderer>().material;
        }
        else
        {
            _material = _renderer.material;
        }
        EnemyMovement enemyMovement =  GetComponent<EnemyMovement>();
        enemyMovement.SetMaterial(_material);
    }

    private void HandleHealthUpdated(float oldHealth, float newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }
    
    public void DealDamage(int damageAmount)
    {
        if (_currentHealth <= 0) { return; }
        float health = _currentHealth;
        _currentHealth = Mathf.Max(_currentHealth - damageAmount, 0);

        LeanTween.cancel(gameObject);
        HandleHealthUpdated(maxHealth, health);

        LeanTween.value( gameObject, 0.6f, 0f, 0.15f).setOnUpdate( (float val)=>{
            _material.SetFloat("_DamageAmount", val);
        } );

        LeanTween.value( gameObject, health, _currentHealth, 0.15f).setOnUpdate( (float val)=>{
            HandleHealthUpdated(maxHealth, val);
        } );

        if (_currentHealth > 0) { return; }

        //death of enemy
        playerManager.IncreaseResource(resourceDrop);
        playerManager.ScoreIncrease(scoreValue);
        EnemyDeath();
    }

    public void DoDelayedDealDamage(float damageAmount, float duration)
    {
        StartCoroutine(DelayedDealDamage(damageAmount, duration));
    }

    IEnumerator DelayedDealDamage(float damageAmount, float duration)
    {
        yield return new WaitForSeconds (duration);
        this.DealDamage(80);
        StopCoroutine("DelayedDealDamage");
    }

    public void EnemyDeath()
    {
        if(this.gameObject != null)
        {
            Destroy(this.gameObject, 0.15f);         //place this first, so if invoke errors, it will still be destroyed
            ServerOnDie?.Invoke(this.gameObject);
        }
    }

    public void SetCurrentHealth(float health)
    {
        maxHealth = health;
        _currentHealth = maxHealth;
        Debug.Log("Boss health is set to" + health + ", now health is " + _currentHealth);
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

