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
    private bool isActivated = false, canShoot = false;

    public GameObject firePoint;
    [SerializeField] private GameObject crystal;

    void Start()
    {
        lineRenderer.SetPosition(1, firePoint.transform.position);
        lineRenderer.SetPosition(0, firePoint.transform.position);
        
        towerAim = GetComponent<TowerAim>();
        towerLevel = GetComponent<TowerLevel>();
    }


    void LateUpdate()
    {
        if(towerAim.enemyList.Count <= 0 && (laserHit.isPlaying || lineRenderer.enabled)) 
		{
			lineRenderer.SetPosition(1, firePoint.transform.position);
			lineRenderer.enabled = false;
			laserHit.Stop(true);
            CrystalDeactivate();
            isActivated = false;
			return;
		} 
        else if (towerAim.targetedEnemyHealth != null)
        {
            
            if(!isActivated)
            {
                isActivated = true;
                CrystalActivate();
            }
                
            if(canShoot)
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

        
        Ray ray = new Ray(firePoint.transform.position, firePoint.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            // Debug.Log("Particle laser");
            if(!laserHit.isPlaying)
                laserHit.Play();
            
            laserHit.transform.position = hit.point;
            // Debug.Log(hit.point);
            Vector3 dir = transform.position - hit.point;
            laserHit.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void CrystalActivate()
    {
        LeanTween.value( gameObject, crystal.transform.localPosition.y, 0.3f, 0.2f).setOnUpdate( (float val)=>{
            // Debug.Log("Activate Y: " + val);
            crystal.transform.localPosition = new Vector3(crystal.transform.localPosition.x, val, crystal.transform.localPosition.z);
        } ).setOnComplete(ChangeShootFlag);
    }

    private void CrystalDeactivate()
    {
        LeanTween.cancel(gameObject);
        canShoot = false;
        LeanTween.value( gameObject, crystal.transform.localPosition.y, 0f, 0.2f).setOnUpdate( (float val)=>{
            // Debug.Log("Deactivate Y: " + val);
            crystal.transform.localPosition = new Vector3(crystal.transform.localPosition.x, val, crystal.transform.localPosition.z);
        } );
        
    }

    private void ChangeShootFlag()
    {
        canShoot = true;

        LeanTween.rotateY(gameObject, 360, 10f).setFrom(0).setRepeat(-1);
    }

    private void OnDrawGizmos()
    {
        // Debug.DrawRay(ray1.origin, ray1.direction * 20, Color.yellow);
        Debug.DrawRay(firePoint.transform.position, firePoint.transform.forward * 20, Color.red);
    }
}
