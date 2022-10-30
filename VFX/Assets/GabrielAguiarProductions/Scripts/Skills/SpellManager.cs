using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField] private List<EnemyMovement> enemyMovements;

    void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSpell1()
    {
        Debug.Log("Q or Spell1 is activated");
        if(!playerManager.ReduceCharge(1)) { return; }
        foreach(EnemyMovement enemyMovement in enemyMovements)
        {
            enemyMovement.SlowDown(50f,2f);
        }
    }

    private void OnSpell2()
    {
        
    }

    private void OnSpell3()
    {
        
    }

    private void OnSpell4()
    {
        
    }
}
