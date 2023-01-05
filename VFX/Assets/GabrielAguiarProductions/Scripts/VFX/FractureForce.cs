using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureForce : MonoBehaviour
{
    public static FractureForce Instance;
    [SerializeField] private Rigidbody[] fracturedParts;
    [SerializeField] private int fracturePerLoop;
    // [SerializeField] private int index = 0;
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
        // if(fracturedParts == null)
        if(SaveManager.Instance.checkSaveFile())
            PlayerManager.Instance.LoadBaseFractures();
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
        Initfractures();
        for(int i = 0; i < fracturePerLoop; i++)
        {
            if(fracturedParts.Length <= 0) { return; }

            Vector3 force = (fracturedParts[i].transform.position - transform.position).normalized * breakForce;
            fracturedParts[i].constraints = RigidbodyConstraints.None;
            fracturedParts[i].AddForce(force);
            // fracturedParts[index].gameObject.GetComponent<MeshCollider>().enabled = false;
            // LeanTween.delayedCall(gameObject, 3f, ()=>{    
            fracturedParts[i].gameObject.GetComponent<FadeAway>().Fade();
            //     fracturedParts[index].gameObject.GetComponent<MeshCollider>().enabled = false;
            //     Debug.Log(fracturedParts[index].name);
            // });
            // index++;
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
            if(fracturedParts.Length <= 0) { return; }   
            Destroy(fracturedParts[i].gameObject);
            Debug.Log("destroy fractures");
        }
    }

    // public void setIndex(int healthLost)
    // {
    //     index += healthLost;
    // }
}
