using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToEnemyScript : MonoBehaviour
{
	private Vector3 direction;
	private Quaternion rotation;
	public GameObject firePoint;
	public GameObject effectToSpawn;
	private float timeToFire = 0f;
    public List<GameObject> enemyList;
	[SerializeField]
    private PathManager pathManager;
	private List<GameObject> waypoints;
	public bool collided;
	[SerializeField]
	private bool isMortar;
	[SerializeField]
	private float mortarOffset;		//sphere collider mortar limit is 22
	[SerializeField]
	private float predictionMultiplier;
	private WaitForSeconds updateTime = new WaitForSeconds (0.01f); 
	private EnemyMovement enemyMovement;
	[SerializeField] private TowerLevel towerLevel;
	public List<EnemyHealth> enemyHealthList;			//needs to be public, idk why

	void Start () {	
		pathManager = GameObject.FindObjectOfType<PathManager>();
		waypoints = pathManager.GetWaypointList();
		towerLevel = GetComponent<TowerLevel>();
	}

	private void OnDestroy()
    {
		foreach(EnemyHealth enemyHealth in enemyHealthList)
		{
			enemyHealth.ServerOnDie -= RemoveEnemy;
			enemyHealthList.Remove(enemyHealth);
		}
	}

	void OnTriggerEnter (Collider co)
    {
        if (co.gameObject.tag == "Enemy") {
			//collided = true;
			enemyList.Add(co.gameObject);
			//transform.LookAt(enemyList[0].transform);
			enemyMovement = enemyList[0].GetComponent<EnemyMovement>();
			enemyHealthList.Add(co.gameObject.GetComponent<EnemyHealth>());
			enemyHealthList[enemyHealthList.Count-1].ServerOnDie += RemoveEnemy;
		}
	}

	private void OnTriggerStay(Collider co)
    {
        if (co.gameObject.tag == "Enemy") {
			StartUpdateRay(co.gameObject);
		}
    }

	private void OnTriggerExit(Collider other)
    {
		if (other.gameObject.tag == "Enemy") {
			RemoveEnemy(other.gameObject);
			foreach(EnemyHealth health in enemyHealthList)
			{
				if(health.gameObject == other.gameObject)
					health.ServerOnDie -= RemoveEnemy;
			}
			
		}
	}

	private void RemoveEnemy(GameObject other)
	{
		enemyList.Remove(other);
		enemyHealthList.Remove(other.GetComponent<EnemyHealth>());
		// Debug.Log("deleted " + other.name);
		if(enemyList.Count > 0)
		{
			enemyMovement = enemyList[0].GetComponent<EnemyMovement>();
		}
	}

	public void StartUpdateRay (GameObject enemy){
		//StartCoroutine (UpdateRay(enemy));
		UpdateRay(enemy);
	}

	void UpdateRay (GameObject enemy){
		if(isMortar)
		{
		int temmp = (int)(enemyMovement.enemySpeed * predictionMultiplier) + enemyMovement.pathIndex;
		if(temmp > waypoints.Count-1)
			temmp = waypoints.Count-1;
		Vector3 temp = waypoints[temmp].transform.position;
		//Vector3 temp = enemy.position;
			temp.y += mortarOffset;
		transform.LookAt(temp);
		//firePoint.transform.rotation = this.transform.rotation;
		}
		else
			transform.LookAt(enemyMovement.transform.position);

		if (Time.time >= timeToFire) {
			timeToFire = Time.time + 1f / towerLevel.FireRate;
			SpawnVFX();
		}
		/*yield return updateTime;
		if(!enemyList.Count.Equals(0))
			StartCoroutine (UpdateRay (enemyList[0]));*/
	}

/*
	void RotateToEnemy (GameObject obj, Vector3 destination ) {
		direction = destination - obj.transform.position;
		rotation = Quaternion.LookRotation (direction);
		obj.transform.localRotation = Quaternion.Lerp (obj.transform.rotation, rotation, 1);
	}
*/

	public void SpawnVFX () {
		GameObject vfx;

		if (firePoint != null) {
			vfx = Instantiate (effectToSpawn, firePoint.transform.position, firePoint.transform.rotation);
			//
		}else
			vfx = Instantiate (effectToSpawn);

//		var direction = enemyList[0].transform - this.transform.position;
//		vfx.transform.localRotation = Quaternion.LookRotation(direction);

		if(isMortar)
		{
			vfx.GetComponent<MortarMoveScript>().damageDeal = towerLevel.DamageDeal;
		}
		else
		{
			vfx.GetComponent<ProjectileMoveScript>().target = enemyMovement.gameObject;			//pass enemy and damage value to projectile
			vfx.GetComponent<ProjectileMoveScript>().damageDeal = towerLevel.DamageDeal;
		}

		var ps = vfx.GetComponent<ParticleSystem> ();

		if (vfx.transform.childCount > 0) {
			ps = vfx.transform.GetChild (0).GetComponent<ParticleSystem> ();
		}
	}

	

}
