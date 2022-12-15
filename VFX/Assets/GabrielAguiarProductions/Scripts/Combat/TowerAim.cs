using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAim : MonoBehaviour
{
	public GameObject firePoint;
	private Vector3 direction;
	private Quaternion rotation;
	private float timeToFire = 0f;
    
	[SerializeField] private GameObject partsToRotate;
	[SerializeField] private PathManager pathManager;
	[SerializeField] private bool isMortar;
	[SerializeField] private float mortarOffset;		//sphere collider mortar limit is 22
	[SerializeField] private float predictionMultiplier;
	[SerializeField] private TowerLevel towerLevel;

	public List<GameObject> enemyList;
	public List<EnemyHealth> enemyHealthList;			//needs to be public, idk why
	private List<GameObject> waypoints;

	public EnemyMovement enemyMovement;
	public EnemyHealth targetedEnemyHealth;


	void Start ()
    {	
		pathManager = GameObject.FindObjectOfType<PathManager>();
		waypoints = pathManager.GetWaypointList();
		towerLevel = GetComponent<TowerLevel>();
		InvokeRepeating("CheckEnemy", 1f, 0.2f);
	}

	void Update()
	{
		if(enemyList.Count <= 0) { return; }
		StartRotation();
	}

	private void OnDisable()
    {
		foreach(EnemyHealth enemyHealth in enemyHealthList.ToArray())
		{
			enemyHealth.ServerOnDie -= RemoveEnemy;
			enemyHealthList.Remove(enemyHealth);
		}
	}

	void OnTriggerEnter (Collider co)
    {
        if (co.gameObject.tag == "Enemy") {
			enemyList.Add(co.gameObject);
			enemyHealthList.Add(co.gameObject.GetComponent<EnemyHealth>());
			enemyHealthList[enemyHealthList.Count-1].ServerOnDie += RemoveEnemy;
		}
	}

	private void OnTriggerExit(Collider other)
    {
		if (other.gameObject.tag == "Enemy") {
			RemoveEnemy(other.gameObject);
		}

		foreach(EnemyHealth health in enemyHealthList)
		{
			if(health == null)
				enemyHealthList.Remove(health);
		}
	}

	private void RemoveEnemy(GameObject other)
	{
		enemyList.Remove(other);
		enemyHealthList.Remove(other.GetComponent<EnemyHealth>());
	}

	private void StartRotation ()
    {
		if(isMortar && enemyMovement != null)
		{
		int temmp = (int)(enemyMovement.enemySpeed * predictionMultiplier) + enemyMovement.pathIndex;
		if(temmp > waypoints.Count-1)
			temmp = waypoints.Count-1;

        Vector3 temp = waypoints[temmp].transform.position;
        temp.y += mortarOffset;
		partsToRotate.transform.LookAt(temp);
		}
		else
		{
			if(enemyMovement != null)
				partsToRotate.transform.LookAt(enemyMovement.transform.position);
		}

		// if (Time.time >= timeToFire) {
		// 	timeToFire = Time.time + (1f / towerLevel.FireRate);
		// 	SpawnVFX();
		// }
	}

	private void CheckEnemy()
	{
		if(enemyList.Count > 0)
		{
			foreach(GameObject enemy in enemyList.ToArray())
			{
				if(enemy == null)
				{
					Debug.Log("Enemy missing...");
					RemoveEnemy(enemy);
					
					return;
				}
			}
			enemyMovement = enemyList[0].GetComponent<EnemyMovement>();
			targetedEnemyHealth = enemyList[0].GetComponent<EnemyHealth>();
		}
		else
		{
			enemyMovement = null;
			targetedEnemyHealth = null;
		}
	}

	public void fireDelay(float time)
	{
		timeToFire += time;
	}

	// private void Laser()
	// {
	// 	if(!lineRenderer.enabled) { lineRenderer.enabled = true; }

	// 	transform.LookAt(enemyMovement.transform.position);
	// 	lineRenderer.SetPosition(0, firePoint.transform.position);
	// 	lineRenderer.SetPosition(1, enemyMovement.transform.position);
	// 	if (Time.time >= timeToFire) 
	// 	{
	// 		timeToFire = Time.time + (1f / towerLevel.FireRate);
	// 		targetedEnemyHealth.DealDamage(towerLevel.DamageDeal);
	// 		towerLevel.XpIncrease(1);
	// 	}

		
	// 	Ray ray = new Ray(firePoint.transform.position, transform.forward);
    //     if(Physics.Raycast(ray, out RaycastHit hit))
    //     {
    //         if(!laserHit.isPlaying)
	// 			laserHit.Play();
	// 		laserHit.transform.position = hit.point;
	// 		Vector3 dir = transform.position - hit.point;
	// 		laserHit.transform.rotation = Quaternion.LookRotation(dir);
    //     }
	// }
}
