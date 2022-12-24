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
    private bool isDead;
    private float originalSpeed;
    private Material _material;
    
    void Start()
    {
        pathManager = GameObject.Find("GameManager").GetComponentInChildren<PathManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        waypoints = pathManager.GetWaypointList();
        pathIndex = PathManager.Instance.GetWaypointsAmount()-1;
        target = waypoints[pathIndex];
        //target = pathManager.GetWaypoint(pathIndex);
        pathsAmount = pathManager.GetWaypointsAmount();
        // pathsAmount++;  //waypointIndex calculation is still bizzarre
        originalSpeed = enemySpeed;
        _material = GetComponent<Renderer>().material;
    }

    void FixedUpdate()
    {
        // if(pathIndex < pathsAmount)
            Move();
    }

    private void Move()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        transform.Translate(dir.normalized * enemySpeed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.transform.position) <= 0.15f)
        {
            pathIndex--;
            // if(pathIndex > waypoints.Count)
            // {
                target = waypoints[pathIndex];
                transform.LookAt(target.transform);
                //target = pathManager.GetWaypoint(pathIndex);
            // }
            // else if(!isDead)
            // {
            //     isDead = true;
            //     playerManager.ReducePlayerHealth(1);
            //     enemyHealth.EnemyDeath();
            // }
        }
    }

    public EnemyHealth GetHealth()
    {
        return enemyHealth;
    }

    void OnTriggerEnter (Collider co)
    {
        // Debug.Log("Collided with" + co.gameObject.tag);
        if (co.gameObject.tag == "Base") {
            // Debug.Log("in");
			if(!isDead)
            {
                isDead = true;
                playerManager.ReducePlayerHealth(1);
                enemyHealth.EnemyDeath();
            }
		}
	}

    public void DoSlowDown(float reduction, float duration)
    {
        StartCoroutine(SlowDown(reduction, duration));
    }

    //reduction takes in value of 0.0% - 100%
    IEnumerator SlowDown(float reduction, float duration)
    {
        // LeanTween.cancel(gameObject);
        LeanTween.value( gameObject, 0.4f, 0f, duration).setOnUpdate( (float val)=>{
            _material.SetFloat("_FrozeAmount", val);
        } );
        // float temp = enemySpeed;
        enemySpeed *= ((100 - reduction)/100);
        var cubeRenderer = GetComponent<Renderer>();
        yield return new WaitForSeconds (duration);
        enemySpeed = originalSpeed;

        StopCoroutine("SlowDown");
    }
}
