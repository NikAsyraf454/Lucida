using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    [SerializeField] private bool slow = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Effect(GameObject enemy)
    {
        if(slow)
        {
            if(enemy.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
            {
                enemyMovement.DoSlowDown(50f,1.5f);
            }
        }
    }
}
