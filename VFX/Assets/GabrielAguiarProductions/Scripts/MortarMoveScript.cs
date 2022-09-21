using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarMoveScript : MonoBehaviour
{
    [SerializeField]
	private float speed;
    [SerializeField]
	private Vector3 offset;
    [SerializeField]
	private Rigidbody rb;
	public GameObject hitPrefab;
	public AudioClip shotSFX;
	public AudioClip hitSFX;
	public List<GameObject> trails;
    public List<EnemyHealth> enemyHealthInRange;
	public int damageDeal = 0;
	

    void Start()
    {
		rb = GetComponent <Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void FixedUpdate () {	
		if (speed != 0 && rb != null)
			rb.position += (transform.forward + offset)  * (speed * Time.deltaTime);
	}

    void OnCollisionEnter (Collision co) 
	{
		foreach(EnemyHealth enemyHealth in enemyHealthInRange)
		{
			enemyHealth.DealDamage(damageDeal);
		}
		

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

    void OnTriggerEnter (Collider co) 
	{
		if (co.gameObject.tag == "Enemy")
		{
			
			enemyHealthInRange.Add(co.gameObject.GetComponent<EnemyHealth>());
			//Debug.Log(co.gameObject.name);
		}
	}

	//OnTriggerExit() delete enemy thats not in range (optional)
    
}
