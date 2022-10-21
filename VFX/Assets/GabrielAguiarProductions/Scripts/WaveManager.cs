using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour, ISaveable
{
    public static WaveManager Instance;
    [SerializeField] private List<GameObject> enemyPrefabList;
    public List<EnemyMovement> enemyList;
    public List<Waves> waves;
    private PathManager pathManager;
    private GameObject spawnPoint;
    [SerializeField] private float timeBetweenWave = 5f;
    private float timer = 10f;
    public int waveIndex = 0;
    private int groupIndex = 0;
    private PlayerManager playerManager;
    private bool canSpawnNext = false;
    private bool lastOfWave = false;
    public List<EnemyHealth> enemyHealthList;

    void Start()
    {
        Instance = this;
        pathManager = GetComponent<PathManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        spawnPoint = pathManager.GetWaypoint(0);
        canSpawnNext = true;
        //SpawnWave();
        //waveIndex++;
        // Debug.Log(waves.Count);
        // Debug.Log(waves[0].enemyGroup.Count);
        // Debug.Log(waves[0].enemyGroup[0].amount);
    }

    void Update()
    {
        if(timer <= 0f && canSpawnNext)
        {
            if(waveIndex == waves.Count) { return; }
            
            canSpawnNext = false;
            lastOfWave = false;
            SpawnWave();
        }

        timer -= Time.deltaTime;
    }

    void SpawnWave()
    {
        Debug.Log("Spawning the wave: " + waveIndex);
        for(groupIndex = 0; groupIndex < waves[waveIndex].enemyGroup.Count; groupIndex++)
        {
            StartCoroutine(SpawnGroup(groupIndex, 0));
        }

        waveIndex++;
    }

    IEnumerator SpawnGroup(int groupNum, int j)
    {
        for(; j < waves[waveIndex].enemyGroup[groupNum].amount; j++)        //j is for enemy amount, i is for enemyGroup index
        {
            //SpawnEnemy(waves[waveNum].enemyGroup[i].enemyPrefab);
            SpawnEnemy(waves[waveIndex].enemyGroup[groupNum].enemyId, spawnPoint.transform.position, spawnPoint.transform.rotation);

            if(groupNum == waves[waveIndex].enemyGroup.Count-1 && j == waves[waveIndex].enemyGroup[groupNum].amount-1)
            {
                Debug.Log("Last of wave " + waveIndex + "group num: " + groupNum);
                lastOfWave = true;
            }

            yield return new WaitForSeconds(1f / waves[waveIndex].enemyGroup[groupNum].rate);
        }

        

        //enemyCount++;
    }

    private void SpawnEnemy(int id, Vector3 pos, Quaternion rotation)
    {
        GameObject enemy = Instantiate(enemyPrefabList[id], pos, rotation);
        enemy.GetComponent<EnemyHealth>().playerManager = playerManager;
        enemyList.Add(enemy.GetComponent<EnemyMovement>());
        enemyHealthList.Add(enemy.GetComponent<EnemyHealth>());
        enemyHealthList[enemyHealthList.Count-1].ServerOnDie += RemoveEnemy;
    }

    private void RemoveEnemy(GameObject other)
	{
        enemyList.Remove(other.GetComponent<EnemyMovement>());
		enemyHealthList.Remove(other.GetComponent<EnemyHealth>());

        if(enemyHealthList.Count <= 0 && lastOfWave)
        {
            timer = timeBetweenWave;
            canSpawnNext = true;
            Debug.Log("timerstart: " + timer);

            Debug.Log("Saving...");
            SaveManager.Instance.Save();
        }
    }

    private void OnDestroy()
    {
		foreach(EnemyHealth enemyHealth in enemyHealthList)
		{
			enemyHealth.ServerOnDie -= RemoveEnemy;
			enemyHealthList.Remove(enemyHealth);
		}
	}

    #region Save and Load

    public object CaptureState()
    {
        List<int> enemyId = new List<int>();
        List<float> enemyPos = new List<float>();
        List<int> enemyPathIndex = new List<int>();
        List<int> enemyHealth = new List<int>();
        List<float> enemySpeed = new List<float>();
        int waveNum;
        float timerSave;
        foreach(EnemyMovement enemyMovement in enemyList)
        {
            enemyId.Add(enemyMovement.enemyId);
            enemyPos.Add(enemyMovement.gameObject.transform.position.x);
            enemyPos.Add(enemyMovement.gameObject.transform.position.y);
            enemyPos.Add(enemyMovement.gameObject.transform.position.z);

            enemyPathIndex.Add(enemyMovement.pathIndex);
            enemyHealth.Add(enemyMovement.enemyHealth.CurrentHealth);
            Debug.Log("saving:" + enemyMovement.enemyHealth.CurrentHealth);
            enemySpeed.Add(enemyMovement.enemySpeed);
        }

        waveNum = waveIndex;
        timerSave = timer;
        return new SaveData
        {
            enemyId = enemyId,
            enemyPos = enemyPos,
            enemypPathIndex = enemyPathIndex,
            enemyHealth = enemyHealth,
            enemySpeed = enemySpeed,
            waveNum = waveNum,
            timerSave = timerSave
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        UpdateLoadProperties(saveData);
    }

    private void UpdateLoadProperties(SaveData saveData)         //if any properties needed to be updated for UI or etc
    {
        List<int> enemyId = new List<int>();
        List<float> enemyPos = new List<float>();
        List<int> enemyPathIndex = new List<int>();
        List<int> enemyHealth = new List<int>();
        List<float> enemySpeed = new List<float>();
        enemyId = saveData.enemyId;
        enemyPos = saveData.enemyPos;
        enemyPathIndex = saveData.enemypPathIndex;
        enemyHealth = saveData.enemyHealth;
        enemySpeed = saveData.enemySpeed;

        int j = 0;
        foreach(int i in enemyId)
        {
            SpawnEnemy(i, new Vector3(enemyPos[j*3], enemyPos[j*3+1], enemyPos[j*3+2]), Quaternion.identity);
            enemyList[j].pathIndex = enemyPathIndex[j];
            enemyHealthList[j].CurrentHealth = enemyHealth[j];
            Debug.Log("Load:" + enemyHealth[j]);
            enemyList[j].enemySpeed = enemySpeed[j];
            j++;
        }
        
        //load wave savings
        waveIndex = saveData.waveNum;
        timer = saveData.timerSave;
    }

    [Serializable]
    private struct SaveData
    {
        public List<int> enemyId;
        public List<float> enemyPos;
        public List<int> enemypPathIndex;
        public List<int> enemyHealth;
        public List<float> enemySpeed;
        //wave saving
        public int waveNum;
        public float timerSave;
        //public List<float> waves or spawning sequence;
    }

    #endregion
}
