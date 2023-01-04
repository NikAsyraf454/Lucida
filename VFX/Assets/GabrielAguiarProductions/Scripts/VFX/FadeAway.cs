using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    public Material material;
    public MeshCollider co;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().GetComponent<Material>();
        co = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fade()
    {
        
        LeanTween.delayedCall(gameObject, 1f, ()=>{    
            co.enabled = false;
            Debug.Log("collider disabled");
        });

        RemoveFacture();
    }

    private void RemoveFacture()
    {
        LeanTween.delayedCall(gameObject, 2f, ()=>{    
            Debug.Log("Destroyed");
            Destroy(gameObject); 
        });
        
    }
}
//damageplayer 1