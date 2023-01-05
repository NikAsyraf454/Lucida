using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMortar : MonoBehaviour
{
    
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

		
        vfx.GetComponent<MortarMoveScript>().damageDeal = towerLevel._damageDeal;
        towerLevel.XpIncrease(3);		//increase xp of tower every shot
		
		var ps = vfx.GetComponent<ParticleSystem> ();

		if (vfx.transform.childCount > 0) {
			ps = vfx.transform.GetChild (0).GetComponent<ParticleSystem> ();
		}
    }
}
