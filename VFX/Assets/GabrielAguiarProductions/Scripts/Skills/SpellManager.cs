using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellManager : MonoBehaviour
{
    private PlayerManager playerManager;
    [SerializeField] private List<EnemyMovement> enemyMovements;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float areaRadius = 5f;

    void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSpell1()     //Slowdown Enemy
    {
        // Debug.Log("Q or Spell1 is activated");
        if(!playerManager.ReduceCharge(1)) { return; }
        Collider[] colliders = AreaDetection();

        foreach(Collider co in colliders)
        {
            Debug.Log(co.gameObject.name);
            if(co.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
            {
                enemyMovement.DoSlowDown(50f,2f);
            }
        }
    }

    private void OnSpell2()     //Increase tower damage
    {
        //decide on all tower or area selection
        //if all tower
        if(!playerManager.ReduceCharge(2)) { return; }
        List<TowerLevel> towerList = TowerManager.Instance.spawnedTowerList;

        foreach(TowerLevel towerLevel in towerList)
        {
            towerLevel.DoIncreaseDamage(50f, 10f);
        }

    }

    private void OnSpell3()     //Increase Resource multiplier
    {
        PlayerManager.Instance.DoIncreaseMultiplier(50f, 15f);
    }

    private void OnSpell4()     //Damage enemy
    {
        if(!playerManager.ReduceCharge(2)) { return; }
        Collider[] colliders = AreaDetection();

        foreach(Collider co in colliders)
        {
            if(co.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                enemyHealth.DealDamage(30);
            }
        }
    }

    private Collider[] AreaDetection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        {
            return(Physics.OverlapSphere(hit.point, 5f));
        }
        else
        {
            return null;
        }
        
    }

    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        {
            // Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Gizmos.DrawWireSphere(hit.point, 5f);
        }

        // Ray ray1 = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        // Debug.DrawRay(ray1.origin, ray1.direction * 20, Color.yellow);
    }
}
