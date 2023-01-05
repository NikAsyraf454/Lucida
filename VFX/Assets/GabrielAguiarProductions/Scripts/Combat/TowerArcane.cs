using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArcane : MonoBehaviour
{
    [SerializeField] private AudioClip _clip_ShootArcane;

    [SerializeField] private TowerAim towerAim;
    [SerializeField] private TowerLevel towerLevel;
    public GameObject effectToSpawn;
    [SerializeField] private float timeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        towerAim = GetComponent<TowerAim>();
        towerLevel = GetComponent<TowerLevel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (towerAim.enemyList.Count > 0 && Time.time >= timeToFire) 
        {
            timeToFire = Time.time + (1f / towerLevel.FireRate);
            if(towerAim.enemyMovement != null)
            Shoot();
            SoundManager.Instance.PlaySound(_clip_ShootArcane);
        }
    }

    public void Shoot()
    {
        GameObject vfx;

		if (towerAim.firePoint != null) {
			vfx = Instantiate (effectToSpawn, towerAim.firePoint.transform.position, towerAim.firePoint.transform.rotation);
			//
		}else
			vfx = Instantiate (effectToSpawn);

		
        vfx.GetComponent<ProjectileMoveScript>().target = towerAim.enemyMovement.gameObject;			//pass enemy and damage value to projectile
        vfx.GetComponent<ProjectileMoveScript>().damageDeal = towerLevel._damageDeal;/* = towerLevel.DamageDeal * towerLevel.Level; */
        towerLevel.XpIncrease(1);		//increase xp of tower every shot
		
		var ps = vfx.GetComponent<ParticleSystem> ();

		if (vfx.transform.childCount > 0) {
			ps = vfx.transform.GetChild (0).GetComponent<ParticleSystem> ();
		}
    }
}
