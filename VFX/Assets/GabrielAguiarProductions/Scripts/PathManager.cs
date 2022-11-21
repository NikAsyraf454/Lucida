using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathManager : MonoBehaviour/* , ISaveable */
{
    public List<Node> nodeList;
    public List<bool> canBuiltTemp;
    public List<GameObject> waypoints;
    [SerializeField]
    private GameObject basePrefab;
    [SerializeField]
    private GameObject pathPrefab;
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private GameObject waypointPrefab;
    [SerializeField]
    private int lenght;
    [SerializeField]
    private int height;
    private int leftTurn;
    private int rightTurn;
    private int randomSeed = 10;
    private int pathIndexX = 0;
    private int pathIndexZ = 0;
    int[,] tilePlacement = new int[100,100];
    private WaitForSeconds updateTime = new WaitForSeconds (0.5f); 

    void Start()
    {
        leftTurn = rightTurn = (height/2);
        //int[,] tilePlacement = new int[lenght,height];
        pathIndexZ = height/2;

        GeneratePath();

        for(int i=0 ; i<lenght ; i++)
        {
            for(int j=0 ; j<height ; j++)
            {
                if(tilePlacement[i,j] == 1)
                {
                    SpawnPathTile(new Vector3(i,0,j));
                }
                else if(tilePlacement[i,j] == 2)
                {
                    SpawnTile(nodePrefab, new Vector3(i,0,j));
                }
            }
        }

        waypoints.Reverse();
    }

    void Update()
    {

    }

    void GeneratePath()
    {
        //left z=-1, right z=+1
        //tile matrix: 1 is path, 2 is grass

        for(int i=0 ; i<lenght ; i++)
        {
            for(int j=0 ; j<height ; j++)
            {
                tilePlacement[i,j] = 2;
            }
        }

        SpawnTile(basePrefab, new Vector3(2,0.1f,height/2));   //base
        tilePlacement[1,height/2] = 1;
        tilePlacement[2,height/2] = 1;

        tilePlacement[0,height/2] = 1;
        tilePlacement[0,(height/2)+1] = 1;
        tilePlacement[0,(height/2)-1] = 1;
        tilePlacement[1,(height/2)+1] = 1;
        tilePlacement[1,(height/2)-1] = 1;
        tilePlacement[2,(height/2)+1] = 1;
        tilePlacement[2,(height/2)-1] = 1;

        SpawnWaypoint(2,height/2);

        pathIndexX = 2;
        GoStraight();
        GoStraight();
        //SpawnWaypoint(2,height/2);

        while(pathIndexX < lenght-1)
        {
            NewPath();
        }
        //GoStraight();           //make the last line straight
    }

    void GoStraight()
    {
        pathIndexX++;
        tilePlacement[pathIndexX,pathIndexZ] = 1;
        SpawnWaypoint(pathIndexX,pathIndexZ);
    }

    void SetWaypoint()
    {

    }

	void NewPath (){
        UnityEngine.Random.InitState(randomSeed);
        randomSeed++;
        switch(UnityEngine.Random.Range(1,4))
        {
            case 1:     //turn left
                if(leftTurn > 0)
                {
                    //SpawnWaypoint(pathIndexX,pathIndexZ);
                    pathIndexZ--;
                    tilePlacement[pathIndexX,pathIndexZ] = 1;
                    SpawnWaypoint(pathIndexX,pathIndexZ);
                    rightTurn++;
                    leftTurn--;
                    GoStraight();
                    GoStraight();
                }else{
                    NewPath();
                }
                break;
            case 2:     //go straight
                GoStraight();
                break;
            case 3:     //turn right
                if(rightTurn > 0)
                {
                    //SpawnWaypoint(pathIndexX,pathIndexZ);
                    pathIndexZ++;
                    tilePlacement[pathIndexX,pathIndexZ] = 1;
                    SpawnWaypoint(pathIndexX,pathIndexZ);
                    rightTurn--;
                    leftTurn++;
                    GoStraight();
                    GoStraight();
                }else{
                    NewPath();
                }
                break;
        }
	}

    void SpawnPathTile(Vector3 pos)
    {
        GameObject temp = Instantiate(pathPrefab, pos, Quaternion.identity, this.gameObject.transform);
    }
        
    void SpawnTile(GameObject prefab, Vector3 pos)
    {
        GameObject temp = Instantiate(prefab, pos, prefab.transform.rotation, this.gameObject.transform);
        Node node = temp.GetComponentInChildren<Node>();
        if(prefab == nodePrefab)
        {
            nodeList.Add(node);
        }
    }

    void SpawnWaypoint(int x, int z)
    {
        GameObject temp = Instantiate(waypointPrefab, new Vector3(x, 0.5f, z), Quaternion.identity, this.gameObject.transform);
        waypoints.Add(temp);
    }

    public GameObject GetWaypoint(int waypointIndex)
    {
        return waypoints[waypointIndex];
    }

    public int GetWaypointsAmount()
    {
        return waypoints.Count;
    }

    // public GameObject GetBuildings(int buildingsIndex)
    // {
    //     return buildings[buildingsIndex];
    // }

    public List<GameObject> GetWaypointList()
    {
        return waypoints;
    }


    /* #region Save and Load
        
    
    public object CaptureState()
    {
        List<bool> temp = new List<bool>();
        //int i = 0;
        foreach(Node node in nodeList)
        {
            temp.Add(node.canBuild);
        }

        return new SaveData
        {
            canBuiltList = temp
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        canBuiltTemp = saveData.canBuiltList;
        UpdateLoadProperties();
    }

    private void UpdateLoadProperties()         //if any properties needed to be updated for UI or etc
    {
        int i = 0;
        foreach(Node node in nodeList)
        {
            node.canBuild = canBuiltTemp[i];
            i++;
        }
    }

    [Serializable]
    private struct SaveData
    {
        public List<bool> canBuiltList;
        // public int xp;
    }
    
    #endregion */
}
