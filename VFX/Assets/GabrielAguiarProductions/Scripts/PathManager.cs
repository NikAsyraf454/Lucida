using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathManager : MonoBehaviour, ISaveable
{
    public static PathManager Instance;
    public List<Node> nodeList;
    public List<bool> canBuiltTemp;
    public List<GameObject> waypoints;
    public int sectionUnlocked = 0;
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
    [SerializeField] private int originalSeed = -1;
    [SerializeField] private int randomSeed = -1;
    private int pathIndexX = 0;
    private int pathIndexZ = 0;
    int[,] tilePlacement = new int[100,100];
    private WaitForSeconds updateTime = new WaitForSeconds (0.5f);

    void Awake()
    {
        Instance = this;


    }

    void Start()
    {
        if(!SaveManager.Instance.checkSaveFile())
        {
            LeanTween.delayedCall(gameObject, 0.5f, ()=>{
                randomSeed = originalSeed;    
                InitPath();
            });
        }
    }

    // void Start()
    private void InitPath()
    {
        // Debug.Log("before randomize " + originalSeed);
        if(originalSeed == -1)
        {
            originalSeed = UnityEngine.Random.Range(1,1000);
            randomSeed = originalSeed;
            Debug.Log("seed randomize");
        }

        

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

        // waypoints.Reverse();
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

        SpawnTile(basePrefab, new Vector3(1,0.8f,height/2));   //base
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
        if(pathIndexX >= lenght-1) { return; }
        pathIndexX++;
        tilePlacement[pathIndexX,pathIndexZ] = 1;
        SpawnWaypoint(pathIndexX,pathIndexZ);
    }

    // void SetWaypoint()
    // {

    // }

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
        // Debug.Log(x + z);
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

    public void NextSection()
    {
        lenght += 15;
        sectionUnlocked++;
        
        int lastIndexX = pathIndexX+1;

        for(int i=pathIndexX+1 ; i<lenght ; i++)
        {
            for(int j=0 ; j<height ; j++)
            {
                tilePlacement[i,j] = 2;
            }
        }


        // pathIndexX--;
        GoStraight();

        while(pathIndexX < lenght-1)
        {
            NewPath();
        }

        for(int i=lastIndexX ; i<lenght ; i++)
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

        // waypoints.Reverse();
        SaveManager.Instance.Save();
    }


    #region Save and Load
        
    
    public object CaptureState()
    {
        // int seed;
        // Debug.Log("saving seed: " + originalSeed);
        // Debug.Log("saving Section: " + sectionUnlocked);
        return new SaveData
        {
            seed = originalSeed,
            sectionUnlocked = sectionUnlocked
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        originalSeed = saveData.seed;
        int temp = saveData.sectionUnlocked;
        // Debug.Log("Section from save: " + sectionUnlocked);
        UpdateLoadProperties(temp);
    }

    private void UpdateLoadProperties(int sectionUnlocked)         //if any properties needed to be updated for UI or etc
    {
        randomSeed = originalSeed;
        InitPath();
        PlayerManager.Instance.LoadBaseFractures();
        if(sectionUnlocked <= 0) { return; }

        for(int i = 0; i < sectionUnlocked; i++)
        {
            NextSection();
        }
        
    }

    [Serializable]
    private struct SaveData
    {
        public int seed;
        public int sectionUnlocked;
        // public int xp;
    }
    
    #endregion
}
