using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public int enemyId;
    [SerializeField] private PathManager pathManager;
    [SerializeField] public int pathIndex = 0;
    [SerializeField]
    public float enemySpeed = 1f;
    [SerializeField] private GameObject target;
    private int pathsAmount;
    [SerializeField] public EnemyHealth enemyHealth;
    private PlayerManager playerManager;
    private List<GameObject> waypoints;
    
    void Start()
    {
        pathManager = GameObject.Find("GameManager").GetComponent<PathManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        waypoints = pathManager.GetWaypointList();
        target = waypoints[pathIndex];
        //target = pathManager.GetWaypoint(pathIndex);
        pathsAmount = pathManager.GetWaypointsAmount();
        pathsAmount++;  //waypointIndex calculation is still bizzarre
    }

    void FixedUpdate()
    {
        if(pathIndex < pathsAmount)
            Move();
    }

    private void Move()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        transform.Translate(dir.normalized * enemySpeed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.transform.position) <= 0.15f)
        {
            pathIndex++;
            if(pathIndex < pathsAmount)
                target = waypoints[pathIndex];
                //target = pathManager.GetWaypoint(pathIndex);
            else
            {
                enemyHealth.EnemyDeath();
                playerManager.ReducePlayerHealth();
            }
        }
    }

    public EnemyHealth GetHealth()
    {
        return enemyHealth;
    }
}
