using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    private Color startColor;
    [SerializeField] private Color hoverColor;
    private Renderer rend;
    public PathManager pathManager;
    public TowerManager towerManager;
    public bool canBuild = true;
    public TowerLevel towerLevel;
    [SerializeField] private GameObject[] EnvProp;
    [Range(0.0f, 1.0f)]
    public float propPercentage;
    [SerializeField] private int minProp, maxProp;
    [SerializeField] private Vector2 minPropPosition, maxPropPosition;
    [SerializeField] private List<GameObject> propSpawned;

    private void Start()
    {
        //mainCamera = Camera.main;
        //buildingCollider = building.GetComponent<BoxCollider>();
        rend = GetComponentInChildren<Renderer>();
        startColor = rend.material.color;
        pathManager = GameObject.FindObjectOfType<PathManager>();
        towerManager = pathManager.gameObject.GetComponentInParent<TowerManager>();
        SpawnProp();
    }

    private void Update()
    {            
        
    }

    void OnMouseDown()
    {
        //only for prototyping, later change this to event action
        if(towerManager.towerInstance == null) { return; }
        if(!canBuild) { return; }
        //return true if cannot build, false if can build
        canBuild = towerManager.BuildTower( new Vector3(transform.position.x, 0f, transform.position.z));
        
        if(!canBuild)
        {
            foreach(GameObject prop in propSpawned)
            {
                Destroy(prop);
            }
        }
    }

    void OnMouseEnter()
    {
        if(towerManager.towerInstance == null) { return; }
        if(!canBuild) { return; }
        rend.material.color = hoverColor;
        //
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
        //
    }

    public void TowerDestroyed()
    {
        canBuild = true;
    }

    private void UpdateBuildingPreview()
    {

    }

    private void SpawnProp()
    {
        float temp = UnityEngine.Random.Range(0f, 100f);
        // Debug.Log(temp + ", " + propPercentage);
        // if(temp > propPercentage) { return; }

        int propAmount = UnityEngine.Random.Range(minProp, maxProp);
        for(int i = 0; i < propAmount; i++)
        {
            Vector3 pos = new Vector3(
                transform.parent.position.x + UnityEngine.Random.Range(minPropPosition.x, maxPropPosition.x),
                transform.parent.position.y + 0.3f,
                transform.parent.position.z + UnityEngine.Random.Range(minPropPosition.y, maxPropPosition.y)
            );
            var prop = Instantiate(EnvProp[UnityEngine.Random.Range(0, EnvProp.Length)], pos, Quaternion.identity, transform.parent.GetComponent<Transform>());
            propSpawned.Add(prop);
        }
    }
}

