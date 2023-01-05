using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureForce : MonoBehaviour
{
    public static FractureForce Instance;
    [SerializeField] private Rigidbody[] fracturedParts;
    [SerializeField] private int fracturePerLoop;
    [SerializeField] private int index = 0;
    [SerializeField] private float breakForce;
    [SerializeField] private float breakRadius;

    void Awake()
    {
        Instance = this; 
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // PlayerManager.Instance.castleFracture = this;
        if(fracturedParts == null)
            Initfractures();
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("left mouse clicked");
        //     explodeFracture();
        // }
            

    }

    public void explodeFracture()
    {
        
        for(int i = 0; i < fracturePerLoop; i++)
        {
            if(index > fracturedParts.Length-1) { return; }

            Vector3 force = (fracturedParts[index].transform.position - transform.position).normalized * breakForce;
            fracturedParts[index].constraints = RigidbodyConstraints.None;
            fracturedParts[index].AddForce(force);
            // fracturedParts[index].gameObject.GetComponent<MeshCollider>().enabled = false;
            // LeanTween.delayedCall(gameObject, 3f, ()=>{    
            fracturedParts[index].gameObject.GetComponent<FadeAway>().Fade();
            //     fracturedParts[index].gameObject.GetComponent<MeshCollider>().enabled = false;
            //     Debug.Log(fracturedParts[index].name);
            // });
            index++;
        }

    }

    public void Initfractures()
    {
        fracturedParts = GetComponentsInChildren<Rigidbody>();
        // Debug.Log("initfractured parts");
    }

    public void removeFracture(int loop)
    {
        Initfractures();
        loop = loop * 3;
        for(int i = 0; i < loop; i++)
        {

            if(index > fracturedParts.Length-1) { return; }   
            Destroy(fracturedParts[index].gameObject);
            index++;
        }
    }

    public void setIndex(int healthLost)
    {
        index += healthLost;
    }
}
