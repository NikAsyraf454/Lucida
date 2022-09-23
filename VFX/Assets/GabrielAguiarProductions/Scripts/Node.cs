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

    private void Start()
    {
        //mainCamera = Camera.main;
        //buildingCollider = building.GetComponent<BoxCollider>();
        rend = GetComponentInChildren<Renderer>();
        startColor = rend.material.color;
        pathManager = GameObject.FindObjectOfType<PathManager>();
        towerManager = pathManager.gameObject.GetComponent<TowerManager>();
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
        canBuild = towerManager.BuildTower( new Vector3(transform.position.x, 0.64f, transform.position.z));
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

    private void UpdateBuildingPreview()
    {

    }
}

