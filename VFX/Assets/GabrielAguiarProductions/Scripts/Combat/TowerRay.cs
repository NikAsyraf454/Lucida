using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRay : MonoBehaviour
{
	private bool laserOn = false;
	[SerializeField] private LineRenderer lineRenderer;
	[SerializeField] private ParticleSystem laserHit;

    [SerializeField] private TowerAim towerAim;
    [SerializeField] private TowerLevel towerLevel;
    // public GameObject effectToSpawn;
    [SerializeField] private float timeToFire = 0f;

    public GameObject firePoint;

    void Start()
    {
        lineRenderer.SetPosition(1, firePoint.transform.position);
        lineRenderer.SetPosition(0, firePoint.transform.position);
        
        towerAim = GetComponent<TowerAim>();
        towerLevel = GetComponent<TowerLevel>();
    }


    void LateUpdate()
    {
        if(towerAim.enemyList.Count <= 0 && laserHit.isPlaying) 
		{
			lineRenderer.SetPosition(1, firePoint.transform.position);
			lineRenderer.enabled = false;
			laserHit.Stop(true);
			return;
		} 
        else if (towerAim.targetedEnemyHealth != null)
        {
            Laser();
        }
    }

	private void Laser()
	{
            // if(enemyMovement == null) { return; }
        if(!lineRenderer.enabled) { lineRenderer.enabled = true; }

        // transform.LookAt(towerAim.enemyMovement.transform);
        lineRenderer.SetPosition(0, firePoint.transform.position);
        lineRenderer.SetPosition(1, towerAim.enemyMovement.transform.position);
        if (Time.time >= timeToFire) 
        {
            timeToFire = Time.time + (1f / towerLevel.FireRate);
            // SpawnVFX();
            towerAim.targetedEnemyHealth.DealDamage(towerLevel.DamageDeal);
            towerLevel.XpIncrease(1);
        }

        
        Ray ray = new Ray(firePoint.transform.position, transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            // Debug.Log("Particle laser");
            if(!laserHit.isPlaying)
                laserHit.Play();
            laserHit.transform.position = hit.point;
            Vector3 dir = transform.position - hit.point;
            laserHit.transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
