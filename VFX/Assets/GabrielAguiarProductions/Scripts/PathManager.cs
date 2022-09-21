using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public List<GameObject> buildings;
    public List<GameObject> waypoints;
    [SerializeField]
    private GameObject basePrefab;
    [SerializeField]
    private GameObject pathPrefab;
    [SerializeField]
    private GameObject grassPrefab;
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

    void Awake()
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
                    SpawnTile(grassPrefab, new Vector3(i,0,j));
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

        SpawnTile(basePrefab, new Vector3(2,0.6f,height/2));   //base
        tilePlacement[2,height/2] = 1;
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
        Random.InitState(randomSeed);
        randomSeed++;
        switch(Random.Range(1,4))
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
        GameObject temp = Instantiate(prefab, pos, Quaternion.identity, this.gameObject.transform);
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

    public GameObject GetBuildings(int buildingsIndex)
    {
        return buildings[buildingsIndex];
    }

    public List<GameObject> GetWaypointList()
    {
        return waypoints;
    }
}
