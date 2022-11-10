using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveScript : MonoBehaviour {

	public float speed;
	[Tooltip("From 0% to 100%")]
	// public float accuracy;
	public float fireRate;
	public GameObject muzzlePrefab;
	public GameObject hitPrefab;
	public AudioClip shotSFX;
	public AudioClip hitSFX;
	public List<GameObject> trails;
	public GameObject target = null;		//enemy basically
	public int damageDeal = 0;

	private float speedRandomness;
	private Vector3 offset;
	//private bool collided;
	private Rigidbody rb;
	private EnemyHealth enemyHealth;
	public float lifeSpan = 3f;

	[SerializeField] private ProjectileEffect projectileEffect;

	void Start () {	
		rb = GetComponent <Rigidbody> ();

		//used to create a radius for the accuracy and have a very unique randomness
/* 		if (accuracy != 100) {
			accuracy = 1 - (accuracy / 100);

			for (int i = 0; i < 2; i++) {
				var val = 1 * Random.Range (-accuracy, accuracy);
				var index = Random.Range (0, 2);
				if (i == 0) {
					if (index == 0)
						offset = new Vector3 (0, -val, 0);
					else
						offset = new Vector3 (0, val, 0);
				} else {
					if (index == 0)
						offset = new Vector3 (0, offset.y, -val);
					else
						offset = new Vector3 (0, offset.y, val);
				}
			}
		}
 */			
		if (muzzlePrefab != null) {
			var muzzleVFX = Instantiate (muzzlePrefab, transform.position, Quaternion.identity);
			muzzleVFX.transform.forward = gameObject.transform.forward + offset;
			var ps = muzzleVFX.GetComponent<ParticleSystem>();
			if (ps != null)
				Destroy (muzzleVFX, ps.main.duration);
			else {
				var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Destroy (muzzleVFX, psChild.main.duration);
			}
		}

		if (shotSFX != null && GetComponent<AudioSource>()) {
			GetComponent<AudioSource> ().PlayOneShot (shotSFX);
		}

		Destroy(gameObject, lifeSpan);
	}

	void FixedUpdate () {	
		if (speed != 0 && rb != null)
		{
			if(target != null)
				transform.LookAt(target.transform.position);
			rb.position += (transform.forward + offset)  * (speed * Time.deltaTime);
		}
	}

	void OnCollisionEnter (Collision co) {
		if (co.gameObject.tag == "Enemy"){

			enemyHealth = co.gameObject.GetComponent<EnemyHealth>();
			enemyHealth.DealDamage(damageDeal);
			if(projectileEffect != null) { projectileEffect.Effect(co.gameObject); /* Debug.Log("Have projectile Effect"); */ }

			
			if (shotSFX != null && GetComponent<AudioSource>()) {
				GetComponent<AudioSource> ().PlayOneShot (hitSFX);
			}

			if (trails.Count > 0) {
				for (int i = 0; i < trails.Count; i++) {
					trails [i].transform.parent = null;
					var ps = trails [i].GetComponent<ParticleSystem> ();
					if (ps != null) {
						ps.Stop ();
						Destroy (ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
					}
				}
			}
		
			speed = 0;
			GetComponent<Rigidbody> ().isKinematic = true;

			ContactPoint contact = co.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
			Vector3 pos = contact.point;

			if (hitPrefab != null) {
				var hitVFX = Instantiate (hitPrefab, pos, rot) as GameObject;

				var ps = hitVFX.GetComponent<ParticleSystem> ();
				if (ps == null) {
					var psChild = hitVFX.transform.GetChild (0).GetComponent<ParticleSystem> ();
					Destroy (hitVFX, psChild.main.duration);
				} else
					Destroy (hitVFX, ps.main.duration);
			}

			StartCoroutine (DestroyParticle (0f));
		}
	}

	public IEnumerator DestroyParticle (float waitTime) {

		if (transform.childCount > 0 && waitTime != 0) {
			List<Transform> tList = new List<Transform> ();

			foreach (Transform t in transform.GetChild(0).transform) {
				tList.Add (t);
			}		

			while (transform.GetChild(0).localScale.x > 0) {
				yield return new WaitForSeconds (0.01f);
				transform.GetChild(0).localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				for (int i = 0; i < tList.Count; i++) {
					tList[i].localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				}
			}
		}
		
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}
}
